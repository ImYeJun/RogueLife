using System;
using System.ComponentModel;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class StrengthenMuscle : DisposableBattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public StrengthenMuscle() {}
        private StrengthenMuscle(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }
        
        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new StrengthenMuscle(context, owner, state);
        }

        public void StrengthenDamage(RequestHurtEntityBattleAction requestHurtEntity, BattleContext context)
        {
            var source = requestHurtEntity.Source;

            if (source.Caster is not BattleEntity sourceEntity) { return; }
            if (sourceEntity != owner) { return; }

            OnExecuted();
            requestHurtEntity.AddDamage(state.StackCount * 5);
            RequestExpire();
        }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<RequestHurtEntityBattleAction>(StrengthenDamage, PipelinePhaseStep.EARLY);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<RequestHurtEntityBattleAction>(StrengthenDamage);
        }
    }
}