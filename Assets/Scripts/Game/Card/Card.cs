#nullable enable

using System;
using System.Collections.Generic;
using Battle.Cards.Casters;
using Battle.HurtSources;
using UnityEngine;

public class Card : ICardBehaviourOwner, IReadOnlyBattleCard
{
    private DateTime obatinDate;
    private CardData data;
    private CardBattleBehaviour behaviourInstance;
    private string currentName;
    private CardType currentType;
    private CardAttribute currentAttribute;
    private CardRarity currentRarity;
    private bool isReflectionApplied;
    private int baseActionCost;
    private HashSet<CardCostModifier> costModifiers;

    public CardData Data { get => data; }
    public Sprite Background { get => data.Background; }
    public string CurrentName { get => currentName; }
    public string CurrentDescription { get => isReflectionApplied ? data.RelfectionActivatedDescription : data.Description; }
    public string NormalEffectDescription { get => data.Description; }
    public string ReflectionEffectDescription { get => data.RelfectionActivatedDescription; }
    public CardType CurrentType { get => currentType; }
    public CardAttribute CurrentAttribute { get => currentAttribute; }
    public CardRarity CurrentRarity { get => currentRarity; }
    public bool IsReflectionApplied { get => isReflectionApplied; }
    public CardTargetType TargetType { get => isReflectionApplied ? behaviourInstance.ReflectionTargetType : behaviourInstance.TargetType; }
    public int BaseActionCost { get => baseActionCost; }
    public HashSet<CardCostModifier> CostModifiers { get => costModifiers; }
    public int CurrentActionCost { 
        get {
            int result = baseActionCost;
            
            foreach (var modifier in costModifiers)
            {
                result += modifier.Delta;
            }

            return Mathf.Max(result, 0);
        }
    }
    public DateTime ObtainData { get => obatinDate; }

    public CardData GetAsData => data;

    public event Action OnCostChanged;
    public event Action OnReflectionChanged;

    public Card(CardEntity entity)
    {
        obatinDate = new DateTime();

        data = entity.Data;
        behaviourInstance = entity.CloneBattleBehaviour(this);

        currentName = data.CardName;
        currentType = data.Type;
        currentAttribute = data.Attribute;
        currentRarity = data.Rarity;
        isReflectionApplied = false;

        baseActionCost = data.ActionCost;
        costModifiers = new HashSet<CardCostModifier>();
    }
    
    public Card(Card card, bool isForBattleStart = false)
    {
        obatinDate = card.obatinDate;

        data = card.Data;
        behaviourInstance = card.CloneBattleBehaviour(this);

        currentName = card.CurrentName;
        currentType = card.CurrentType;
        currentAttribute = card.CurrentAttribute;
        currentRarity = card.CurrentRarity;
        isReflectionApplied = card.IsReflectionApplied;

        baseActionCost = card.CurrentActionCost;
        //TODO SHIT!! Refactor this fxxcked code.Create a class named BattleCard owning Card class as composition.
        costModifiers = isForBattleStart ? new HashSet<CardCostModifier>(card.CostModifiers) : new HashSet<CardCostModifier>();
    }

    public Card(CardEntity entity, CardSaveData cardSaveData)
    {
        if (entity.Data.Id != cardSaveData.cardId) { throw new InvalidOperationException("[Card] the given arguments' id are not matched"); }
        DateTime date;
        if (!DateTime.TryParse(cardSaveData.obtainDate, out date))
        {
            throw new InvalidOperationException("[Card] The given date format is not valid.");
        }
        obatinDate = date;

        data = entity.Data;
        behaviourInstance = entity.CloneBattleBehaviour(this);

        currentName = cardSaveData.cardName;
        currentType = cardSaveData.type;
        currentAttribute = cardSaveData.attribute;
        currentRarity = cardSaveData.rarity;
        isReflectionApplied = false;

        baseActionCost = cardSaveData.baseActionCost;
        costModifiers = cardSaveData.costModifiers;
    }

    public void AddCostModifier(CardCostModifier modifier)
    {
        if (!costModifiers.Add(modifier))
        {
            UnityEngine.Debug.LogWarning("[Card] The given cost modifier is already exisiting");
        }
        else{
            OnCostChanged?.Invoke();
        }
    }
    public void RemoveCostModifier(CardCostModifier modifier)
    {
        if (!costModifiers.Remove(modifier))
        {
            UnityEngine.Debug.LogWarning("[Card] The given cost modifier is not exisiting");
        }
        else{
            OnCostChanged?.Invoke();
        }
    }

    public void OnDraw(BattleContext context) { behaviourInstance.OnDraw(context); }
    public bool IsAbleToUse(BattleContext context, CardTarget target) { 
        return isReflectionApplied ?
            behaviourInstance.IsAbleToUseReflect(context, target) : 
            behaviourInstance.IsAbleToUse(context, target);
    }
    public void Use(BattleContext context, CardCaster caster, CardTarget targetEntity)
    {
        if (!behaviourInstance.IsTargetValid(targetEntity, context, isReflectionApplied)) { return; }

        if (isReflectionApplied) { 
            behaviourInstance.ExecuteReflection(context, caster, targetEntity);
        }
        else { behaviourInstance.Execute(context, caster, targetEntity); }
    }
    public void Trigger(BattleContext context, CardCaster caster, CardTarget targetEntity, bool isReflection = false)
    {
        if (!behaviourInstance.IsTargetValid(targetEntity, context, isReflection)) { return; }

        if (isReflection) { behaviourInstance.ExecuteReflection(context, caster, targetEntity); }
        else { behaviourInstance.Execute(context, caster, targetEntity); }
    }

    public void ApplyReflection() { 
        isReflectionApplied = true;
        OnReflectionChanged?.Invoke();
    }
    public void UnapplyReflection() { 
        isReflectionApplied = false;
        OnReflectionChanged?.Invoke();
    }

    public BattleHurtSource GetAsHurtSource(CardCaster cardCaster)
    {
        BattleEntity? caster = cardCaster.Caster;
        return new CardSource(this, caster);
    }

    public bool Equals(Card operand) => operand.Data.Equals(data);

    private CardBattleBehaviour CloneBattleBehaviour(ICardBehaviourOwner owner) => behaviourInstance.Clone(owner);
}