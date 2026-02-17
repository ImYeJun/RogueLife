using System;
using UnityEngine;

public class BattleStatusEffect : IBattleStatusEffectState
{
    private BattleStatusEffectData data;
    private int stackCount;
    private int remainTurn;
    private bool isDurationEternal;
    private BattleStatusEffectBehaviour behaviour;
    private Action<BattleStatusEffect> OnExpired;

    private BattleStatusEffect(BattleStatusEffectData data, int stackCount, int remainTurn, bool isDurationEternal)
    {
        this.data = data;
        this.stackCount = stackCount;
        this.remainTurn = remainTurn;
        this.isDurationEternal = isDurationEternal;
    }

    public BattleStatusEffect(BattleStatusEffectData data, int startStackCount, int startRemainTurn) 
    : this(data, startStackCount, startRemainTurn, false) { }

    public BattleStatusEffect(BattleStatusEffectData data, int startStackCount)
    : this(data,startStackCount, Int32.MaxValue, true) { }

    public BattleStatusEffectData Data { get => data;  }
    public int StackCount => stackCount;
    public bool IsDurationEternal => isDurationEternal;
    public int RemainTurn => remainTurn;
    public bool IsExpired => remainTurn <= 0;
    public BattleEntityTrait RequiredTraits => data.RequiredTraits;
    public BattleEntityCondition GrantedCondition => data.GrantedCondition;

    public void OnApplied(BattleContext context, IBattleStatusEffectOwner owner, Action<BattleStatusEffect> onExpired)
    {
        OnExpired = onExpired;

        behaviour = data.CloneBehaviour(context, owner, this);
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
        if (data != newEffect.data) return;

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