using System;
using System.Collections.Generic;
using System.Linq;

public class BattleDeck : IDrawDeckContext, IHandDeckContext, IGraveDeckContext
{
    private List<Card> deck = new List<Card>();
    public Card this[int index]
    {
        get { return deck[index]; }
    }

    public int Count { get => deck.Count; }
    public void SetDeck(List<Card> deck) { this.deck = deck; }
    public void Clear() { deck.Clear();}

    public void AddCard(Card card)
    {
        if (deck.Contains(card))
        {
            throw new InvalidOperationException("Deck already contains the given card");
        }
        
        deck.Add(card);
    }

    public void RemoveCard(Card card)
    {
        if (!deck.Remove(card))
        {
            throw new InvalidOperationException("Deck doesn't contain the given card");
        }
    }

    public List<Card> GetCards()
    {
        return deck;
    }

    public List<Card> GetCardsByCondition(CardRarity rarity, CardAttribute attribute, CardType type)
    {
        return deck.Where(card => 
            (rarity == CardRarity.ANY || card.CurrentRarity == rarity) &&
            (attribute == CardAttribute.ANY || attribute == card.CurrentAttribute) &&
            (type == CardType.ANY || type == card.CurrentType)
        ).ToList();
    }

    public int GetCardsCountByCondition(CardRarity rarity, CardAttribute attribute, CardType type)
    {
        return GetCardsByCondition(rarity, attribute, type).Count;
    }
    
    public Card GetRandomCard(Random random)
    {
        return GetRandomCard(random, CardRarity.ANY, CardAttribute.ANY, CardType.ANY);
    }

    public Card GetRandomCard(Random random, CardRarity rarity, CardAttribute attribite, CardType type)
    {
        if (deck.Count <= 0)
        {
            UnityEngine.Debug.LogWarning("Deck is empty.");
            return null;
        }

        var filtered = deck.Where(card => 
            (rarity == CardRarity.ANY || card.CurrentRarity == rarity) &&
            (attribite == CardAttribute.ANY || card.CurrentAttribute == attribite) &&
            (type == CardType.ANY || card.CurrentType == type)
        ).ToList();

        if (filtered.Count == 0) { 
            UnityEngine.Debug.LogWarning("There's no possible card to draw");
            return null;
        }
        return filtered[random.Next(filtered.Count)];
    }

    public bool HasCard(Card card)
    {
        return deck.Contains(card);
    }
}