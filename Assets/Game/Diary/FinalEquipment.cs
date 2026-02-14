using System;
using System.Collections.Generic;

[Serializable]
public class FinalEquipment
{
    private Dictionary<CardData, List<Card>> finalMainDeck;
    private Dictionary<CardData, List<Card>> finalSideDeck;
    private List<BelongingsData> finalMainBelongings;
    private List<BelongingsData> finalSideBelongings;

    public FinalEquipment(FinalEquipmentSaveData saveData, CardDatabase cardDatabase, BelongingsDatabase belongingsDatabase)
    {
        finalMainDeck = new Dictionary<CardData, List<Card>>();
        finalSideDeck = new Dictionary<CardData, List<Card>>();
        finalMainBelongings = new List<BelongingsData>();
        finalSideBelongings = new List<BelongingsData>();

        foreach (var pair in saveData.finalMainDeck)
        {
            var data = cardDatabase.GetData(pair.Key);
            if (data == null) { throw new InvalidOperationException($"[FinalEquipment] Failed to get card data, Id : {pair.Key}"); }

            finalMainDeck[data] = new List<Card>();

            foreach(var cardSaveData in pair.Value)
            {
                finalMainDeck[data].Add(new Card(data, cardSaveData));
            }
        }

        foreach (var pair in saveData.finalSideDeck)
        {
            var data = cardDatabase.GetData(pair.Key);
            if (data == null) { throw new InvalidOperationException($"[FinalEquipment] Failed to get card data, Id : {pair.Key}"); }

            finalSideDeck[data] = new List<Card>();

            foreach(var cardSaveData in pair.Value)
            {
                finalSideDeck[data].Add(new Card(data, cardSaveData));
            }
        }

        foreach (var belongingsId in saveData.finalMainBelongings)
        {
            var data = belongingsDatabase.GetData(belongingsId);
            if (data == null) { throw new InvalidOperationException($"[FinalEquipment] Failed to get belongings data, Id : {belongingsId}"); }

            finalMainBelongings.Add(data);
        }

        foreach (var belongingsId in saveData.finalSideBelongings)
        {
            var data = belongingsDatabase.GetData(belongingsId);
            if (data == null) { throw new InvalidOperationException($"[FinalEquipment] Failed to get belongings data, Id : {belongingsId}"); }

            finalSideBelongings.Add(data);
        }
    }

    public FinalEquipment(Dictionary<CardData, List<Card>> finalMainDeck, Dictionary<CardData, List<Card>> finalSideDeck, List<BelongingsData> finalMainBelongings, List<BelongingsData> finalSideBelongings)
    {
        this.finalMainDeck = finalMainDeck;
        this.finalSideDeck = finalSideDeck;
        this.finalMainBelongings = finalMainBelongings;
        this.finalSideBelongings = finalSideBelongings;
    }

    public IReadOnlyDictionary<CardData, List<Card>> FinalMainDeck { get => finalMainDeck; }
    public IReadOnlyDictionary<CardData, List<Card>> FinalSideDeck { get => finalSideDeck; }
    public IReadOnlyList<BelongingsData> FinalMainBelongings { get => finalMainBelongings; }
    public IReadOnlyList<BelongingsData> FinalSideBelongings { get => finalSideBelongings; }
}