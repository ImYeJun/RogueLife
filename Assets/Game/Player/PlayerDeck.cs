using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck
{
    public const int MAX_COPIES_PER_CARD = 3;
    private Dictionary<CardData, List<Card>> mainDeck = new Dictionary<CardData, List<Card>>();
    private Dictionary<CardData, List<Card>> sideDeck = new Dictionary<CardData, List<Card>>();

    public IReadOnlyDictionary<CardData, List<Card>> MainDeck { get => mainDeck; }
    public IReadOnlyDictionary<CardData, List<Card>> SideDeck { get => sideDeck; }

    public bool TryObtainCard(Card card)
    {
        if (!sideDeck.ContainsKey(card.Data)) { sideDeck.Add(card.Data, new List<Card>()); }

        if (sideDeck[card.Data].Count == MAX_COPIES_PER_CARD) { return false; }

        sideDeck[card.Data].Add(card);
        return true;
    } 

    public bool TryMoveCard(Card card, DeckType from, DeckType to)
    {
        Dictionary<CardData, List<Card>> fromDeck = GetDeck(from);
        Dictionary<CardData, List<Card>> toDeck = GetDeck(to);

        if (fromDeck == null || toDeck == null)
        {
            Debug.Log($"DeckTypes {from} or {to} are not valid.");
            return false;
        }
        if (!HasCard(card, fromDeck))
        {
            Debug.Log($"{from} doesn't contain the given card");
            return false;
        }
        if (HasCard(card, toDeck))
        {
            Debug.Log($"{to} already contains the given card");
            return false;
        }

        CardData cardData = card.Data;

        fromDeck[cardData].Remove(card);
        if (!toDeck.ContainsKey(cardData)) { toDeck.Add(cardData, new List<Card>()); }
        toDeck[cardData].Add(card);

        return true;
    }

    private bool HasCard(Card card, Dictionary<CardData, List<Card>> deck)
    {
        if (!deck.ContainsKey(card.Data)) { return false ; }
        return deck[card.Data].Contains(card);
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