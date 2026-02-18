#nullable enable

using System;
using System.ComponentModel;
using Battle.HurtSource;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class Drunk : DisposableBattleStatusEffectBehaviour
    {[Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Drunk() {}
        private Drunk(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }
        

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribePostObserver<RequestHurtEntityBattleAction>(DrunkKick);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribePostObserver<RequestHurtEntityBattleAction>(DrunkKick);
        }

        public void DrunkKick(RequestHurtEntityBattleAction requestHurtEntityBattleAction, BattleContext context)
        {
            if (requestHurtEntityBattleAction.Target != owner) { return; }

            var source = requestHurtEntityBattleAction.Source;
            if (source.Caster is not BattleEntity sourceEntity) { return; }

            context.ActionScheduler.Enqueue(new RequestHurtEntityBattleAction(owner.GetAsHurtSource(), state.StackCount * 5, sourceEntity));
            RequestExpire();
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new Drunk(context, owner, state);
        }
    }
}