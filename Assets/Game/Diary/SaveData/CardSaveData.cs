using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class CardSaveData
{
    public string cardId;
    public string cardName;
    public string description;
    public CardType type;
    public CardAttribute attribute;
    public CardRarity rarity;
    public int actionCost;

    public CardSaveData(Card origin)
    {
        cardId = origin.Data.Id;
        description = origin.CurrentName;
        type = origin.CurrentType;
        attribute = origin.CurrentAttribute;
        rarity = origin.CurrentRarity;
        actionCost = origin.CurrentActionCost;
    }

    [JsonConstructor]
    public CardSaveData(string cardId, string cardName, string description, CardType type, CardAttribute attribute, CardRarity rarity, int actionCost)
    {
        this.cardId = cardId;
        this.cardName = cardName;
        this.description = description;
        this.type = type;
        this.attribute = attribute;
        this.rarity = rarity;
        this.actionCost = actionCost;
    }
}