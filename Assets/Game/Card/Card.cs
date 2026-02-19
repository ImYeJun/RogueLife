#nullable enable

using System;
using Battle.Cards.Casters;
using Battle.HurtSources;
using UnityEngine;

public class Card : ICardBehaviourOwner
{
    private CardData data;
    private CardBattleBehaviour behaviourInstance;
    private string currentName;
    private string currentDescription;
    private CardType currentType;
    private CardAttribute currentAttribute;
    private CardRarity currentRarity;
    private int currentActionCost;
    private bool isReflectionApplied;

    public CardData Data { get => data; }
    public string CurrentName { get => currentName; }
    public string CurrentDescription { get => currentDescription; }
    public CardType CurrentType { get => currentType; }
    public CardAttribute CurrentAttribute { get => currentAttribute; }
    public CardRarity CurrentRarity { get => currentRarity; }
    public int CurrentActionCost { get => currentActionCost; }
    public bool IsReflectionApplied { get => isReflectionApplied; }
    public CardTargetType TargetType { get => isReflectionApplied ? behaviourInstance.ReflectionTargetType : behaviourInstance.TargetType; }

    public Card(CardData data)
    {
        this.data = data;
        behaviourInstance = this.data.CloneBattleBehaviour(this);

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
        behaviourInstance = data.CloneBattleBehaviour(this);

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
        behaviourInstance = data.CloneBattleBehaviour(this);

        currentName = cardSaveData.cardName;
        currentDescription = cardSaveData.description;
        currentType = cardSaveData.type;
        currentAttribute = cardSaveData.attribute;
        currentRarity = cardSaveData.rarity;
        currentActionCost = cardSaveData.actionCost;
        isReflectionApplied = false;
    }

    public void OnDraw(BattleContext context) { behaviourInstance.OnDraw(context); }
    public bool IsAbleToUse(BattleContext context, CardTarget target) { 
        return isReflectionApplied ?
            behaviourInstance.IsAbleToUseReflect(context, target) : 
            behaviourInstance.IsAbleToUse(context, target);
    }
    public void Execute(BattleContext context, CardCaster caster, CardTarget targetEntity)
    {
        if (!behaviourInstance.IsTargetValid(targetEntity, context, isReflectionApplied)) { return; }

        if (isReflectionApplied) { 
            behaviourInstance.ExecuteReflection(context, caster, targetEntity);
            UnapplyReflection();
        }
        else { behaviourInstance.Execute(context, caster, targetEntity); }
    }

    public void ApplyReflection() { isReflectionApplied = true; }
    public void UnapplyReflection() { isReflectionApplied = false; }

    public BattleHurtSource GetAsHurtSource(CardCaster cardCaster)
    {
        BattleEntity? caster = cardCaster.Caster;
        return new CardSource(this, caster);
    }

    public bool Equals(Card operand) => operand.Data.Equals(data);
}