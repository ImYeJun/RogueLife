using System;
using UnityEngine;

public class Card
{
    private CardData data;
    private CardBattleBehaviour battleBehaviourInstance;
    private string currentName;
    private string currentDescription;
    private CardType currentType;
    private CardAttribute currentAttribute;
    private CardRarity currentRarity;
    private int currentActionCost;
    private bool isReflectionApplied = false;

    public CardData Data { get => data; }
    public string CurrentName { get => currentName; }
    public string CurrentDescription { get => currentDescription; }
    public CardType CurrentType { get => currentType; }
    public CardAttribute CurrentAttribute { get => currentAttribute; }
    public CardRarity CurrentRarity { get => currentRarity; }
    public int CurrentActionCost { get => currentActionCost; }
    public bool IsReflectionApplied { get => isReflectionApplied; }
    public CardTargetType TargetType => data.TargetType;

    public Card(CardData data)
    {
        this.data = data;
        battleBehaviourInstance = this.data.CloneBattleBehaviour();

        currentName = data.CardName;
        currentDescription = data.Description;
        currentType = data.Type;
        currentAttribute = data.Attribute;
        currentRarity = data.Rarity;
        currentActionCost = data.ActionCost;
        isReflectionApplied = false;
    }

    public Card(Card card)
    {
        data = card.Data;
        battleBehaviourInstance = data.CloneBattleBehaviour();

        currentName = card.CurrentName;
        currentDescription = card.CurrentDescription;
        currentType = card.CurrentType;
        currentAttribute = card.CurrentAttribute;
        currentRarity = card.CurrentRarity;
        currentActionCost = card.CurrentActionCost;
        isReflectionApplied = card.IsReflectionApplied;
    }

    public Card(CardData cardData, CardSaveData cardSaveData)
    {
        if (cardData.Id != cardSaveData.cardId) { throw new InvalidOperationException("[Card] the given arguments' id are not matched"); }

        data = cardData;

        currentName = cardSaveData.cardName;
        currentDescription = cardSaveData.description;
        currentType = cardSaveData.type;
        currentAttribute = cardSaveData.attribute;
        currentRarity = cardSaveData.rarity;
        currentActionCost = cardSaveData.actionCost;
        isReflectionApplied =false;
    }

    public void OnDraw(BattleContext context) { battleBehaviourInstance.OnDraw(context); }
    public bool IsAbleToUse(BattleContext context, CardTarget target) { return battleBehaviourInstance.IsAbleToUse(context, target); }
    public void Execute(BattleContext context, CardTarget targetEntity)
    {
        if (isReflectionApplied) { 
            battleBehaviourInstance.ExecuteReflection(context, targetEntity);
            UnapplyReflection();
        }
        else { battleBehaviourInstance.Execute(context, targetEntity); }
    }

    public void ApplyReflection() { isReflectionApplied = true; }
    public void UnapplyReflection() { isReflectionApplied = false; }

    public bool Equals(Card operand) => operand.Data.Equals(data);
}