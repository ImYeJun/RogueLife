using System;

[Serializable]
public abstract class DisposableBattleStatusEffectBehaviour : BattleStatusEffectBehaviour
{
    protected DisposableBattleStatusEffectBehaviour() {}

    protected DisposableBattleStatusEffectBehaviour(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
    : base(context, owner, state) { }

    public sealed override void ActivateEffect()
    {
        PerformAction();

        state.RequestExpired();
    }

    public abstract void PerformAction();
}
