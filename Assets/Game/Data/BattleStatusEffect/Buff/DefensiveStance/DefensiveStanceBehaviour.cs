using System;
using System.ComponentModel;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class DefensiveStance : BattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DefensiveStance() {}
        private DefensiveStance(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new DefensiveStance(context, owner, state);
        }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<RequestHurtEntityBattleAction>(ReduceDamage);
        }

        public void ReduceDamage(RequestHurtEntityBattleAction requestHurtEntityBattleAction, BattleContext context)
        {
            if (requestHurtEntityBattleAction.Target != owner) { return; }

            requestHurtEntityBattleAction.ReduceDamage(state.StackCount * 5);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<RequestHurtEntityBattleAction>(ReduceDamage);
        }
    }
}