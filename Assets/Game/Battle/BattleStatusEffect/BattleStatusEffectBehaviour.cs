using System;
using UnityEngine;

[Serializable]
public abstract class BattleStatusEffectBehaviour
{
    protected BattleContext context;
    protected IBattleStatusEffectOwner owner;
    protected IBattleStatusEffectState state;

    protected BattleStatusEffectBehaviour() {}

    protected BattleStatusEffectBehaviour(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
    {
        this.context = context;
        this.owner = owner;
        this.state = state;
    }

    public abstract void OnApplied();
    public abstract void OnRemoved(bool isOwnerDied = false);
    public abstract void ActivateEffect();
    public abstract BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state);
}