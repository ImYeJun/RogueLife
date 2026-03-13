using System;
using UnityEngine;

[Serializable]
public abstract class BattleStatusEffectBehaviour
{
    protected BattleContext context;
    protected IBattleStatusEffectOwner owner;
    protected IBattleStatusEffectState state;

    protected BattleStatusEffectBehaviour() {}

    public event Action Executed;

    protected BattleStatusEffectBehaviour(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
    {
        this.context = context;
        this.owner = owner;
        this.state = state;
    }

    //TODO Make sure subclass always call this method by rectoring with template method pattern
    protected void OnExecuted()
    {
        Executed?.Invoke();
    }
    public abstract void OnApplied();
    public abstract void OnRemoved(bool isOwnerDied = false);
    public abstract void OnMerged();
    public abstract BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state);
}