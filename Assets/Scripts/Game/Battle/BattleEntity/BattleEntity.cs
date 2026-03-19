using System;
using System.Collections.Generic;
using System.Linq;
using Battle.HurtSources;
using UnityEngine;
using ViewEvent.BattleView;

public abstract class BattleEntity : IBattleStatusEffectOwner, IReadOnlyBattleEntity
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
        if (isDead) return;
        context.ActionScheduler.Enqueue(new HealEntityBattleAction(this, amount));
    }
    public void Kill() { 
        if (isDead) return;
        OnDead();
    }

    protected virtual void OnDead()
    {
        if (isDead) return;
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

        if (newEffect.RequiredTraits != BattleEntityTrait.ANY && !trait.HasFlag(newEffect.RequiredTraits))
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
            viewEventPublisher.Publish(new BattleStatusEffectChanged(viewEventPublisher.GetNextSequenceId(), this, existingEffect, existingEffect.RemainTurn, existingEffect.StackCount));
        }
        else
        {
            targetDict[newEffect.Data] = newEffect;
            newEffect.OnApplied(context, this, RequestRemoveStatusEffect);
            newEffect.OnExecuted += OnBattleStatusEffectExecuted;
            viewEventPublisher.Publish(new BattleStatusEffectApplied(viewEventPublisher.GetNextSequenceId(), this, newEffect));
        }

        UpdateCondition();
    }
    
    public bool HasStatusEffect(BattleStatusEffectData data)
    {
        return equippingBuffs.ContainsKey(data) || equippingDebuffs.ContainsKey(data);
    }

    public void OnPlayerTurnEnded(PlayerTurnEndBattleEvent payload)
    {
        if (isDead) return;
        DecreaseStatusEffectDuration();
    }
    public void OnEnemyTurnEnded(EnemyTurnEndBattleEvent payload)
    {
        if (isDead) return;
        DecreaseStatusEffectDuration();
    }
    private void DecreaseStatusEffectDuration()
    {
        if (isDead) return;
        var buffList = equippingBuffs.Values.ToList();
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            var buff = buffList[i];
            buff.DecreaseTurn();

            if (!buff.IsExpired)
            {
                viewEventPublisher.Publish(new BattleStatusEffectChanged(viewEventPublisher.GetNextSequenceId(), this, buff, buff.RemainTurn, buff.StackCount));
            }
        }

        var debuffList = equippingDebuffs.Values.ToList();
        for (int i = debuffList.Count - 1; i >= 0; i--)
        {
            var debuff = debuffList[i];
            debuff.DecreaseTurn();

            if (!debuff.IsExpired)
            {
                viewEventPublisher.Publish(new BattleStatusEffectChanged(viewEventPublisher.GetNextSequenceId(), this, debuff, debuff.RemainTurn, debuff.StackCount));
            }
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
            statusEffect.OnExecuted -= OnBattleStatusEffectExecuted;
            targetDict.Remove(statusEffect.Data);

            UpdateCondition();

            viewEventPublisher.Publish(new BattleStatusEffectRemoved(viewEventPublisher.GetNextSequenceId(), this, statusEffect));
        }
        else
        {
            Debug.LogWarning($"[BattleEntity] The battle entity doesn't contain given status effect {statusEffect.Data.Name}");
        }
    }
    
    private void UpdateCondition()
    {
        if (isDead) return;
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

    private void OnBattleStatusEffectExecuted(IReadOnlyBattleStatusEffect battleStatusEffect)
    {
        if (isDead) return;
        context.EventBus.Publish(new BattleStatusEffectExecutedBattleEvent(this, battleStatusEffect));
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