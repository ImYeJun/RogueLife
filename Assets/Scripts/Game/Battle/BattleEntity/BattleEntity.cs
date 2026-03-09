using System;
using System.Collections.Generic;
using System.Linq;
using Battle.HurtSources;
using UnityEngine;

public abstract class BattleEntity : IBattleStatusEffectOwner
{
    protected IBattleViewEventPublisher viewEventPublisher;
    protected BattleContext context;
    private BattleEntityTrait trait;
    private BattleEntityCondition currentCondition;

    private Dictionary<BattleStatusEffectData, BattleStatusEffect> equippingBuffs = new Dictionary<BattleStatusEffectData, BattleStatusEffect>();
    private Dictionary<BattleStatusEffectData, BattleStatusEffect> equippingDebuffs = new Dictionary<BattleStatusEffectData, BattleStatusEffect>();

    public abstract int CurrentHealth { get; }
    public IReadOnlyDictionary<BattleStatusEffectData, BattleStatusEffect> CurrentBuffs { get => equippingBuffs; }
    public IReadOnlyDictionary<BattleStatusEffectData, BattleStatusEffect> CurrentDebuffs { get => equippingDebuffs; }
    public List<BattleStatusEffect> GetBattleStatusEffects(BattleStatusEffectType type = BattleStatusEffectType.ANY)
    {
        return type switch
        {
            BattleStatusEffectType.BUFF => equippingBuffs.Values.ToList(),
            BattleStatusEffectType.DEBUFF => equippingDebuffs.Values.ToList(),
            BattleStatusEffectType.ANY => equippingBuffs.Values.Concat(equippingDebuffs.Values).ToList(),
            _ => throw new InvalidOperationException($"[BattleEntity] {type} is not valid")
        };
    }
    
    protected bool isDead = false;

    protected BattleEntity(BattleContext context, BattleEntityTrait trait)
    {
        this.context = context;
        this.trait = trait;

        context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(OnPlayerTurnEnded);
        context.EventBus.Subscribe<EnemyTurnEndBattleEvent>(OnEnemyTurnEnded);
    }

    public void SetViewEventPublisher(IBattleViewEventPublisher viewEventPublisher) { this.viewEventPublisher = viewEventPublisher; }

    public abstract bool IsFullHealth { get; }
    public bool IsDead { get => isDead; }
    public BattleEntityTrait Trait { get => trait; }
    public BattleEntityCondition CurrentCondition { get => currentCondition; }
    public abstract int MaxHealth { get; }

    abstract public BattleHurtSource GetAsHurtSource();

    public abstract void RequestHurt(int amount, BattleHurtSource source);
    public abstract void Heal(int amount);
    public void RequestHeal(int amount)
    {
        context.ActionScheduler.Enqueue(new HealEntityBattleAction(this, amount));
    }
    public void Kill() { OnDead(); }

    protected virtual void OnDead()
    {
        isDead = true;

        var buffList = equippingBuffs.Values.ToList();
        foreach (var buff in buffList)
        {
            buff.OnRemoved(true);
        }

        var debuffList = equippingDebuffs.Values.ToList();
        foreach (var debuff in debuffList)
        {
            debuff.OnRemoved(true);
        }

        equippingBuffs.Clear();
        equippingDebuffs.Clear();
        currentCondition = BattleEntityCondition.NONE;

        context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(OnPlayerTurnEnded);
        context.EventBus.Unsubscribe<EnemyTurnEndBattleEvent>(OnEnemyTurnEnded);
    }

    public void RequestApplyStatusEffect(BattleStatusEffect effect)
    {
        if (IsDead) { return; }
        
        context.ActionScheduler.Enqueue(new ApplyEntityStatusEffectBattleAction(this, effect));
    }

    public void ApplyStatusEffect(BattleStatusEffect newEffect)
    {
        if (IsDead) { return; }

        if (!trait.HasFlag(newEffect.RequiredTraits))
        {
            Debug.LogWarning($"[BattleEntity] The entity doesn't fulfilled the required trait. Required : {newEffect.RequiredTraits}, Entity Trait : {trait}");
            return;
        }

        var targetDict = GetEffectDictionary(newEffect.Data.Type);

        if (targetDict == null)
        {
            Debug.LogError($"[BattleEntity] Unsupported Effect Type: {newEffect.Data.Type}");
            return;
        }

        if (targetDict.TryGetValue(newEffect.Data, out var existingEffect))
        {
            existingEffect.MergeWith(newEffect);
        }
        else
        {
            targetDict[newEffect.Data] = newEffect;
            newEffect.OnApplied(context, this, RequestRemoveStatusEffect);
        }

        UpdateCondition();
    }
    
    public bool HasStatusEffect(BattleStatusEffectData data)
    {
        return equippingBuffs.ContainsKey(data) || equippingDebuffs.ContainsKey(data);
    }

    public void OnPlayerTurnEnded(PlayerTurnEndBattleEvent payload)
    {
        DecreaseStatusEffectDuration();
    }
    public void OnEnemyTurnEnded(EnemyTurnEndBattleEvent payload)
    {
        DecreaseStatusEffectDuration();
    }
    private void DecreaseStatusEffectDuration()
    {
        var buffList = equippingBuffs.Values.ToList();
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            var buff = buffList[i];
            buff.DecreaseTurn();
        }

        var debuffList = equippingDebuffs.Values.ToList();
        for (int i = debuffList.Count - 1; i >= 0; i--)
        {
            var debuff = debuffList[i];
            debuff.DecreaseTurn();
        }
    }

    public void RequestRemoveStatusEffect(BattleStatusEffect statusEffect)
    {
        if (IsDead) { return; }
        
        context.ActionScheduler.Enqueue(new RemoveEntityStatusEffect(this, statusEffect));
    }

    public void RemoveStatusEffect(BattleStatusEffect statusEffect)
    {
        if (IsDead) { return; }

        var targetDict = GetEffectDictionary(statusEffect.Data.Type);

        if (targetDict != null && targetDict.ContainsKey(statusEffect.Data))
        {
            statusEffect.OnRemoved();
            targetDict.Remove(statusEffect.Data);

            UpdateCondition();
        }
        else
        {
            Debug.LogWarning("[BattleEntity] The battle entity doesn't contain given status effect");
        }
    }
    
    private void UpdateCondition()
    {
        currentCondition = BattleEntityCondition.NONE;
        foreach (var buff in equippingBuffs.Values)
        {
            currentCondition |= buff.GrantedCondition;
        }
        
        foreach (var debuff in equippingDebuffs.Values)
        {
            currentCondition |= debuff.GrantedCondition;
        }
    }

    private Dictionary<BattleStatusEffectData, BattleStatusEffect> GetEffectDictionary(BattleStatusEffectType type)
    {
        return type switch
        {
            BattleStatusEffectType.BUFF => equippingBuffs,
            BattleStatusEffectType.DEBUFF => equippingDebuffs,
            _ => null
        };
    }
}