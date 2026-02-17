using System;

[Serializable]
public abstract class DisposableBattleStatusEffectBehaviour : BattleStatusEffectBehaviour
{
    protected DisposableBattleStatusEffectBehaviour() {}

    protected DisposableBattleStatusEffectBehaviour(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
    : base(context, owner, state) { }

    public void RequestExpire()
    {
        state.RequestExpired();
    }
}
