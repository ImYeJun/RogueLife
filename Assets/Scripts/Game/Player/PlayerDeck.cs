using System;
using System.Collections.Generic;
using System.Linq;
using Field.Deck.Observers;
using UnityEngine;

public class PlayerDeck : IFieldDeck, IRunDiaryPlayerDeck
{
    private Dictionary<CardData, List<Card>> mainDeck = new Dictionary<CardData, List<Card>>();
    private Dictionary<CardData, List<Card>> sideDeck = new Dictionary<CardData, List<Card>>();
    
    public PlayerDeck(StartDeck startDeck, CardDatabase cardDatabase)
    {
        foreach (var pair in startDeck.StartCards)
        {
            var data = pair.entity;
            var count = pair.count;

            for (int i = 0; i < count; i++)
            {
                var materializedCard = cardDatabase.Materialize(data);
                TryObtainCard(materializedCard);
                TryMoveCard(materializedCard, DeckType.SIDE_DECK, DeckType.MAIN_DECK);
            }
        }
    }
    
    public Dictionary<CardData, List<Card>> GetClonedMainDeck(bool isBattle = false) { 
        var result = new Dictionary<CardData, List<Card>>();
        foreach (var pair in mainDeck)
        {
            var clonedCards = new List<Card>();
            foreach (var card in pair.Value)
            {
                clonedCards.Add(new Card(card, isBattle));
            }
            result[pair.Key] = clonedCards;
        }

        return result;
    }

    public Dictionary<CardData, List<Card>> GetClonedSideDeck()
    {
        var result = new Dictionary<CardData, List<Card>>();
        foreach (var pair in sideDeck)
        {
            var clonedCards = new List<Card>();
            foreach (var card in pair.Value)
            {
                clonedCards.Add(new Card(card));
            }
            result[pair.Key] = clonedCards;
        }

        return result;
    }
    
    public List<CardData> OwingCardData { get => mainDeck.Keys.Union(sideDeck.Keys).ToList(); }
    public int OwingCardVariety { get => OwingCardData.Count(); }

    private int maxCardVariety = Constant.BASE_MAX_DECK_CARD_TYPE_COUNT;

    public IReadOnlyDictionary<CardData, List<Card>> MainDeck { get => mainDeck; }
    public List<Card> MainDeckCards { get
        {
            List<Card> cards = new List<Card>();

            foreach (var pair in mainDeck)
            {
                cards.AddRange(pair.Value);
            }

            return cards;
        }
    }
    public IReadOnlyDictionary<CardData, List<Card>> SideDeck { get => sideDeck; }
    public List<Card> SideDeckCards {
        get
        {
            List<Card> cards = new List<Card>();

            foreach (var pair in sideDeck)
            {
                cards.AddRange(pair.Value);
            }

            return cards;
        }
    }
    public List<Card> OwingCards
    {
        get => MainDeckCards.Concat(SideDeckCards).ToList();
    }
    public int MaxCardVariety { get => maxCardVariety; }

    public bool IsOverflowed => OwingCardVariety > maxCardVariety;

    HashSet<IDeckObserver> deckObservers = new HashSet<IDeckObserver>();
    public event Action<Card> OnCardObtained;
    public event Action<Card> OnCardRemoved;
    public event Action OnCardRemoveRequseted;


    public bool HasEnoughCard(CardData data, int amount = 1)
    {
        int totalCount = 0;
        
        totalCount += mainDeck.ContainsKey(data) ? mainDeck[data].Count : 0;
        totalCount += sideDeck.ContainsKey(data) ? sideDeck[data].Count : 0;

        return totalCount >= amount;
    }

    public bool HasMatchingCard(CardRarity rarity, CardAttribute attribute, CardType type, int leastAmount = 1)
    {
        int matchingCardCount = 0;

        foreach (var cardList in mainDeck.Values)
        {
            foreach (var card in cardList)
            {
                if ((rarity == CardRarity.ANY ||  card.CurrentRarity == rarity) &&
                    (attribute == CardAttribute.ANY || card.CurrentAttribute == attribute) &&
                    (type == CardType.ANY || card.CurrentType == type)
                ) { matchingCardCount++; }
            }
        }
        foreach (var cardList in sideDeck.Values)
        {
            foreach (var card in cardList)
            {
                if ((rarity == CardRarity.ANY ||  card.CurrentRarity == rarity) &&
                    (attribute == CardAttribute.ANY || card.CurrentAttribute == attribute) &&
                    (type == CardType.ANY || card.CurrentType == type)
                ) { matchingCardCount++; }
            }
        }

        return matchingCardCount >= leastAmount;
    }

    public List<Card> GetSpecificCardsByData(CardData data)
    {
        if (!HasCardData(data)) { return null; }

        List<Card> result = new List<Card>();
        
        if (HasCardData(data, DeckType.MAIN_DECK)) { result.AddRange(mainDeck[data]); }
        if (HasCardData(data, DeckType.SIDE_DECK)) { result.AddRange(sideDeck[data]); }

        return result;
    }

    public bool TryObtainCard(Card card)
    {
        if (!sideDeck.ContainsKey(card.Data)) { 
            sideDeck.Add(card.Data, new List<Card>());
        }

        // if (GetCardDataCount(card) >= Constant.BASE_MAX_COPIES_PER_CARD) { 
        //     Debug.Log($"[PlayerDeck] Cannot add card '{card.Data.Id}'. Maximum copies per card ({Constant.BASE_MAX_COPIES_PER_CARD}) exceeded.");
        //     return false;
        // }

        sideDeck[card.Data].Add(card);

        foreach (var observer in deckObservers)
        {
            observer.OnCardEquipped(card);
        }
        OnCardObtained?.Invoke(card);

        if (OwingCardVariety > maxCardVariety) { 
            OnCardRemoveRequseted?.Invoke();
            return false;
        }
        return true;
    } 
    
    public bool TryRemoveCard(Card card, DeckType deckType)
    {
        if(HasCard(card, deckType))
        {
            var deck = GetDeck(deckType);
            deck[card.Data].Remove(card);

            if (deck[card.Data].Count == 0) { deck.Remove(card.Data); }
            
            foreach (var observer in deckObservers)
            {
                observer.OnCardRemoved(card);
            }
            
            OnCardRemoved?.Invoke(card);
            return true;
        }

        return false;
    }
    public bool TryRemoveCard(Card card) { return TryRemoveCard(card, DeckType.SIDE_DECK) || TryRemoveCard(card, DeckType.MAIN_DECK); }
    public bool TryRemoveCardByData(CardData data, int amount)
    {
        if (amount <= 0) return true;
        if (!HasCardData(data)) return false;

        int remainingAmount = amount;

        if (HasCardData(data, DeckType.SIDE_DECK))
        {
            var sideCards = sideDeck[data].ToList(); 
            int removeCount = Mathf.Min(remainingAmount, sideCards.Count);

            for (int i = 0; i < removeCount; i++)
            {
                if (!TryRemoveCard(sideCards[i], DeckType.SIDE_DECK)) return false;
            }
            
            remainingAmount -= removeCount;
        }

        if (remainingAmount > 0 && HasCardData(data, DeckType.MAIN_DECK))
        {
            var mainCards = mainDeck[data].ToList();
            int removeCount = Mathf.Min(remainingAmount, mainCards.Count);

            for (int i = 0; i < removeCount; i++)
            {
                if (!TryRemoveCard(mainCards[i], DeckType.MAIN_DECK)) return false;
            }
            
            remainingAmount -= removeCount;
        }

        return true;
    }
    public bool TryRemoveRandomCard(System.Random random, CardType type, CardAttribute attribute)
    {
        var matchedCards = OwingCards.FindAll(card => 
                                                (type == CardType.ANY || card.CurrentType == type) &&
                                                (attribute == CardAttribute.ANY || card.CurrentAttribute == attribute));

        int removingCardIndex = random.Next(matchedCards.Count);

        return TryRemoveCard(matchedCards[removingCardIndex]);
    }

    public bool TryMoveCard(Card card, DeckType from, DeckType to)
    {
        if (!HasCard(card, from))
        {
            Debug.Log($"[PlayerDeck] {from} doesn't contain the given card");
            return false;
        }
        if (HasCard(card, to))
        {
            Debug.Log($"[PlayerDeck] {to} already contains the given card");
            return false;
        }
        if (to == DeckType.MAIN_DECK)
        {
            if (!HasCardData(card, DeckType.MAIN_DECK) && mainDeck.Count() >= Constant.MAX_MAIN_DECK_CARD_TYPE_COUNT)
            {
                Debug.Log($"[PlayerDeck] {DeckType.MAIN_DECK} is full.");
                return false;
            }
        }

        Dictionary<CardData, List<Card>> fromDeck = GetDeck(from);
        Dictionary<CardData, List<Card>> toDeck = GetDeck(to);

        CardData cardData = card.Data;
        fromDeck[cardData].Remove(card);
        if (fromDeck[cardData].Count() == 0) { fromDeck.Remove(cardData); }
        if (!toDeck.ContainsKey(cardData)) { toDeck.Add(cardData, new List<Card>()); }
        toDeck[cardData].Add(card);

        return true;
    }

    public void RemoveAllCardOfData(CardData data)
    {
        if (HasCardData(data, DeckType.SIDE_DECK))
        {
            var cardsToRemove = sideDeck[data].ToList(); 
            foreach (var card in cardsToRemove)
            {
                TryRemoveCard(card, DeckType.SIDE_DECK);
            }
        }

        if (HasCardData(data, DeckType.MAIN_DECK))
        {
            var cardsToRemove = mainDeck[data].ToList();
            foreach (var card in cardsToRemove)
            {
                TryRemoveCard(card, DeckType.MAIN_DECK);
            }
        }
    }

    public void RegisterDeckobserver(IDeckObserver observer)
    {
        if (deckObservers.Contains(observer))
        {
            Debug.Log("Player Deck already has the observer");
            return;
        }

        deckObservers.Add(observer);
        observer.OnStartObserving(OwingCards);
    }
    public void UnrgisterDeckobserver(IDeckObserver observer)
    {
        if (!deckObservers.Contains(observer))
        {
            Debug.Log("Player Deck doesn't have the observer");
            return;
        }

        observer.OnStopObserving(OwingCards);
        deckObservers.Remove(observer);
    }

    public void IncreaseMaxCardVariety(int amount = 1) { maxCardVariety += amount; }
    public void DecreaseMaxCardVariety(int amount = 1)
    {
        maxCardVariety = Mathf.Max(maxCardVariety - amount, 0);

        if (maxCardVariety < OwingCardVariety)
        {
            int discadCardTypeCount = OwingCardVariety - maxCardVariety;

            for (int i = 0; i <discadCardTypeCount; i++)
            {
                //TODO 유저가 선택해서 한 종류의 카드를 지우게하는 로직 구현하기
            }
        }
    }
    
    public bool HasCard(Card card, DeckType deckType)
    {
        var deck = GetDeck(deckType);

        if (!deck.ContainsKey(card.Data)) { return false; }

        return deck[card.Data].Contains(card);
    }
    public bool HasCard(Card card) { return HasCard(card, DeckType.MAIN_DECK) || HasCard(card, DeckType.SIDE_DECK); }

    public bool HasCardData(CardData cardData, DeckType deckType) { 
        var deck = GetDeck(deckType);
        return deck.ContainsKey(cardData);
    }
    public bool HasCardData(CardData cardData) { return HasCardData(cardData, DeckType.MAIN_DECK) || HasCardData(cardData, DeckType.SIDE_DECK); }
    public bool HasCardData(Card card, DeckType deckType) { return HasCardData(card.Data, deckType); }
    public bool HasCardData(Card card) { return HasCardData(card.Data); }

    public int GetCardDataCount(CardData cardData, DeckType deckType)
    {
        var deck = GetDeck(deckType);

        if (!deck.ContainsKey(cardData)) { return 0; }
        return deck[cardData].Count();
    }
    public int GetCardDataCount(CardData cardData) { return GetCardDataCount(cardData, DeckType.MAIN_DECK) + GetCardDataCount(cardData, DeckType.SIDE_DECK); }
    public int GetCardDataCount(Card card, DeckType deckType) { return GetCardDataCount(card.Data, deckType); }
    public int GetCardDataCount(Card card) { return GetCardDataCount(card.Data); }

    private Dictionary<CardData, List<Card>> GetDeck(DeckType type)
    {
        return type switch
        {
            DeckType.MAIN_DECK => mainDeck,
            DeckType.SIDE_DECK => sideDeck,
            _ => throw new ArgumentOutOfRangeException($"[PlayerDeck] {type} is not valid.")
        };
    }
}