using System;
using System.ComponentModel;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class Toughen : DisposableBattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Toughen() {}
        private Toughen(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }
        
        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new Toughen(context, owner, state);
        }

        public void ReduceDamage(RequestHurtEntityBattleAction requestHurtEntity, BattleContext context)
        {
            if (requestHurtEntity.Target != owner) { return; }

            requestHurtEntity.ReduceDamage(state.StackCount * 5);
            RequestExpire();
        }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<RequestHurtEntityBattleAction>(ReduceDamage);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<RequestHurtEntityBattleAction>(ReduceDamage);
        }
    }
}