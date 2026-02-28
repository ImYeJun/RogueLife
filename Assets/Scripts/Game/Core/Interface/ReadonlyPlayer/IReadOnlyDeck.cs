using System;
using System.Collections.Generic;

public interface IReadOnlyDeck
{
    public IReadOnlyDictionary<CardData, List<Card>> MainDeck { get; }
    public IReadOnlyDictionary<CardData, List<Card>> SideDeck { get; }

    public event Action<IReadOnlyDictionary<CardData, List<Card>>> OnMainDeckChanged;
    public event Action<IReadOnlyDictionary<CardData, List<Card>>> OnSideDeckChanged;
}