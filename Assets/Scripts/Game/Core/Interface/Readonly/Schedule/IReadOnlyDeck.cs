using System;
using System.Collections.Generic;

public interface IReadOnlyDeck
{
    public IReadOnlyDictionary<CardData, List<Card>> MainDeck { get; }
    public IReadOnlyDictionary<CardData, List<Card>> SideDeck { get; }


    public int MaxCardVariety { get; }
}