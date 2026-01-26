using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDeck : IChoiceDeck
{
    private Dictionary<CardData, List<Card>> mainDeck = new Dictionary<CardData, List<Card>>();
    private Dictionary<CardData, List<Card>> sideDeck = new Dictionary<CardData, List<Card>>();
    private HashSet<CardData> owningCardType = new HashSet<CardData>();

    private int maxDeckCardTypeCount = Constant.BASE_MAX_DECK_CARD_TYPE_COUNT;

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

    public bool TryObtainCard(Card card)
    {
        if (!sideDeck.ContainsKey(card.Data)) { 
            if (owningCardType.Count() >= maxDeckCardTypeCount) { return false; }
            sideDeck.Add(card.Data, new List<Card>());
        }

        sideDeck[card.Data].Add(card);
        UpdateOwingCardType();
        return true;
    } 
    
    public bool TryRemoveCard(Card card, DeckType deckType)
    {
        if(HasCard(card, deckType))
        {
            var deck = GetDeck(deckType);
            deck[card.Data].Remove(card);

            if (deck[card.Data].Count == 0) { deck.Remove(card.Data); }
            UpdateOwingCardType();
            return true;
        }

        return false;
    }

    public bool TryRemoveCard(Card card) { return TryRemoveCard(card, DeckType.SIDE_DECK) || TryRemoveCard(card, DeckType.MAIN_DECK); }

    public bool TryMoveCard(Card card, DeckType from, DeckType to)
    {
        if (!HasCard(card, from))
        {
            Debug.Log($"{from} doesn't contain the given card");
            return false;
        }
        if (HasCard(card, to))
        {
            Debug.Log($"{to} already contains the given card");
            return false;
        }
        if (to == DeckType.MAIN_DECK)
        {
            if (!mainDeck.ContainsKey(card.Data) && mainDeck.Count() >= Constant.MAX_MAIN_DECK_CARD_TYPE_COUNT)
            {
                Debug.Log($"{DeckType.MAIN_DECK} is full.");
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

    public void IncreaseMaxCardTypeCount(int amount = 1) { maxDeckCardTypeCount += amount; }

    public void DecreaseMaxCardTypeCount(int amount = 1)
    {
        maxDeckCardTypeCount = Mathf.Max(maxDeckCardTypeCount - amount, 0);

        if (maxDeckCardTypeCount < owningCardType.Count())
        {
            int discadCardTypeCount = owningCardType.Count() - maxDeckCardTypeCount;

            for (int i = 0; i <discadCardTypeCount; i++)
            {
                //TODO 유저가 선택해서 한 종류의 카드를 지우게하는 로직 구현하기
            }
            UpdateOwingCardType();
        }
    }
    
    public bool HasCard(Card card, DeckType deckType)
    {
        var deck = GetDeck(deckType);

        if (deck == null) { return false; }
        if (!deck.ContainsKey(card.Data)) { return false; }

        return deck[card.Data].Contains(card);
    }
    public bool HasCard(Card card) { return HasCard(card, DeckType.MAIN_DECK) || HasCard(card, DeckType.SIDE_DECK); }

    public bool HasCardType(CardData cardData) { return owningCardType.Contains(cardData); }
    public bool HasCardType(Card card) { return HasCardType(card.Data); }

    private void UpdateOwingCardType()
    {
        owningCardType = new HashSet<CardData>();

        owningCardType.UnionWith(mainDeck.Keys);
        owningCardType.UnionWith(sideDeck.Keys);
    } 

    private Dictionary<CardData, List<Card>> GetDeck(DeckType type)
    {
        return type switch
        {
            DeckType.MAIN_DECK => mainDeck,
            DeckType.SIDE_DECK => sideDeck,
            _ => null
        };
    }
}