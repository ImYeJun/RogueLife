using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class WeakenMuscle : DisposableBattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WeakenMuscle() {}
        private WeakenMuscle(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }
        
        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new WeakenMuscle(context, owner, state);
        }

        public void WeakenDamage(RequestHurtEntityBattleAction requestHurtEntity, BattleContext context)
        {
            var source = requestHurtEntity.Source;

            if (source.Caster is not BattleEntity sourceEntity) { return; }
            if (sourceEntity != owner) { return; }

            requestHurtEntity.ReduceDamage(state.StackCount * 5);
            RequestExpire();
        }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<RequestHurtEntityBattleAction>(WeakenDamage);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<RequestHurtEntityBattleAction>(WeakenDamage);
        }
    }
}