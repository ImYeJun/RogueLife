using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class CardSaveData
{
    public string obtainDate;
    public string cardId;
    public string cardName;
    public string description;
    public CardType type;
    public CardAttribute attribute;
    public CardRarity rarity;
    public int baseActionCost;
    public HashSet<CardCostModifier> costModifiers;

    public CardSaveData(Card origin)
    {
        obtainDate = origin.ObtainData.ToString("o");
        cardId = origin.Data.Id;
        description = origin.CurrentName;
        type = origin.CurrentType;
        attribute = origin.CurrentAttribute;
        rarity = origin.CurrentRarity;
        baseActionCost = origin.BaseActionCost;
        costModifiers = origin.CostModifiers;
    }

    [JsonConstructor]
    public CardSaveData(string cardId, string cardName, string description, CardType type, CardAttribute attribute, CardRarity rarity, int baseActionCost, HashSet<CardCostModifier> costModifiers)
    {
        this.cardId = cardId;
        this.cardName = cardName;
        this.description = description;
        this.type = type;
        this.attribute = attribute;
        this.rarity = rarity;
        this.baseActionCost = baseActionCost;
        this.costModifiers = costModifiers;
    }
}