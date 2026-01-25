using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck
{
    private Dictionary<CardData, List<Card>> mainDeck = new Dictionary<CardData, List<Card>>();
    private Dictionary<CardData, List<Card>> sideDeck = new Dictionary<CardData, List<Card>>();

    public IReadOnlyDictionary<CardData, List<Card>> MainDeck { get => mainDeck; }
    public IReadOnlyDictionary<CardData, List<Card>> SideDeck { get => sideDeck; }

    public bool TryObtainCard(Card card)
    {
        if (!sideDeck.ContainsKey(card.Data)) { sideDeck.Add(card.Data, new List<Card>()); }

        if (sideDeck[card.Data].Count == Constant.BASE_MAX_COPIES_PER_CARD) { return false; }

        sideDeck[card.Data].Add(card);
        return true;
    } 

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

        Dictionary<CardData, List<Card>> fromDeck = GetDeck(from);
        Dictionary<CardData, List<Card>> toDeck = GetDeck(to);

        CardData cardData = card.Data;
        fromDeck[cardData].Remove(card);
        if (!toDeck.ContainsKey(cardData)) { toDeck.Add(cardData, new List<Card>()); }
        toDeck[cardData].Add(card);

        return true;
    }

    public bool HasCard(Card card, DeckType deckType)
    {
        Dictionary<CardData, List<Card>> deck = GetDeck(deckType);

        if (deck == null) { return false; }
        if (!deck.ContainsKey(card.Data)) { return false; }

        return deck[card.Data].Contains(card);
    }

    public bool HasCard(Card card)
    {
        return HasCard(card, DeckType.MAIN_DECK) || HasCard(card, DeckType.SIDE_DECK);
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