using System;
using System.ComponentModel;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class IWillKillYou : BattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IWillKillYou() {}
        private IWillKillYou(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }
        
        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new IWillKillYou(context, owner, state);
        }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<RequestHurtEntityBattleAction>(KillYou, PipelinePhaseStep.EARLY);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<RequestHurtEntityBattleAction>(KillYou);
        }

        public void KillYou(RequestHurtEntityBattleAction action, BattleContext context)
        {
            var source = action.Source;
            if (source.Caster != owner) { return; }

            OnExecuted();
            action.AddDamage(state.StackCount * 5);
        }
    }
}