using System;
using System.ComponentModel;
using Battle.HurtSources;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class WaterFist : BattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WaterFist() {}
        private WaterFist(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<RequestHurtEntityBattleAction>(WeakenDamage);
        }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<RequestHurtEntityBattleAction>(WeakenDamage);
        }

        public override void OnMerged() { }

        public void WeakenDamage(RequestHurtEntityBattleAction requestHurtEntityBattleAction, BattleContext context)
        {
            if (requestHurtEntityBattleAction.Source.Caster != owner) { return; }

            OnExecuted();
            requestHurtEntityBattleAction.ReduceDamage(state.StackCount * 5);
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new WaterFist(context, owner, state);
        }
    }
}