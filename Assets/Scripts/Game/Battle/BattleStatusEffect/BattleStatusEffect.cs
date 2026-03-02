using System;
using UnityEngine;

public class BattleStatusEffect : IBattleStatusEffectState
{
    private BattleStatusEffectEntity entity;
    private int stackCount;
    private int remainTurn;
    private bool isDurationEternal;
    private BattleStatusEffectBehaviour behaviour;
    private Action<BattleStatusEffect> OnExpired;

    private BattleStatusEffect(BattleStatusEffectEntity entity, int stackCount, int remainTurn, bool isDurationEternal)
    {
        this.entity = entity;
        this.stackCount = stackCount;
        this.remainTurn = remainTurn;
        this.isDurationEternal = isDurationEternal;
    }

    public BattleStatusEffect(BattleStatusEffectEntity entity, int startStackCount, int startRemainTurn) 
    : this(entity, startStackCount, startRemainTurn, false) { }

    public BattleStatusEffect(BattleStatusEffectEntity entity, int startStackCount)
    : this(entity,startStackCount, Int32.MaxValue, true) { }

    public BattleStatusEffect(BattleStatusEffect origin)
    {
        entity = origin.entity;
        stackCount = origin.stackCount;
        remainTurn = origin.remainTurn;
        isDurationEternal = origin.isDurationEternal;
    }

    public BattleStatusEffectEntity Entity { get => entity; }
    public BattleStatusEffectData Data { get => entity.Data;  }
    public int StackCount => stackCount;
    public bool IsDurationEternal => isDurationEternal;
    public int RemainTurn => remainTurn;
    public bool IsExpired => remainTurn <= 0;
    public BattleEntityTrait RequiredTraits => entity.Data.RequiredTraits;
    public BattleEntityCondition GrantedCondition => entity.Data.GrantedCondition;


    public void OnApplied(BattleContext context, IBattleStatusEffectOwner owner, Action<BattleStatusEffect> onExpired)
    {
        OnExpired = onExpired;

        behaviour = entity.CloneBehaviour(context, owner, this);
        behaviour.OnApplied();
    }
    
    public void OnRemoved(bool isOwnerDied = false)
    {
        behaviour.OnRemoved(isOwnerDied);
    }

    public void DecreaseTurn(int amount = 1)
    {
        if (isDurationEternal) { return; }

        remainTurn = Mathf.Max(remainTurn - amount, 0);

        if (IsExpired)
        {
            OnExpired?.Invoke(this);
        }
    }

    public void RequestExpired()
    {
        OnExpired?.Invoke(this);
    }

    public void MergeWith(BattleStatusEffect newEffect)
    {
        if (entity.Data != newEffect.Data) return;

        stackCount += newEffect.stackCount;

        if (!isDurationEternal)
        {
            if (newEffect.isDurationEternal)
            {
                isDurationEternal = true;
            }
            else
            {
                remainTurn = Mathf.Max(remainTurn, newEffect.remainTurn);
            }
        }
    }
}