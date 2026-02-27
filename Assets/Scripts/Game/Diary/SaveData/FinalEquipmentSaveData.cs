using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class FinalEquipmentSaveData
{
    public Dictionary<string, List<CardSaveData>> finalMainDeck = new Dictionary<string, List<CardSaveData>>();
    public Dictionary<string, List<CardSaveData>> finalSideDeck = new Dictionary<string, List<CardSaveData>>();
    public List<string> finalMainBelongings = new List<string>();
    public List<string> finalSideBelongings = new List<string>();

    public FinalEquipmentSaveData(FinalEquipment origin)
    {
        foreach (var pair in origin.FinalMainDeck)
        {
            var cardId = pair.Key.Id;
            finalMainDeck[cardId] = new List<CardSaveData>();

            foreach (var card in pair.Value)
            {
                finalMainDeck[cardId].Add(new CardSaveData(card));
            }
        }
        foreach (var pair in origin.FinalSideDeck)
        {
            var cardId = pair.Key.Id;
            finalSideDeck[cardId] = new List<CardSaveData>();

            foreach (var card in pair.Value)
            {
                finalSideDeck[cardId].Add(new CardSaveData(card));
            }
        }

        foreach (var belongings in origin.FinalMainBelongings)
        {
            finalMainBelongings.Add(belongings.Id);
        }
        foreach (var belongings in origin.FinalSideBelongings)
        {
            finalSideBelongings.Add(belongings.Id);
        }
    }

    [JsonConstructor]
    public FinalEquipmentSaveData(Dictionary<string, List<CardSaveData>> finalMainDeck, Dictionary<string, List<CardSaveData>> finalSideDeck, List<string> finalMainBelongings, List<string> finalSideBelongings)
    {
        this.finalMainDeck = finalMainDeck;
        this.finalSideDeck = finalSideDeck;
        this.finalMainBelongings = finalMainBelongings;
        this.finalSideBelongings = finalSideBelongings;
    }
}