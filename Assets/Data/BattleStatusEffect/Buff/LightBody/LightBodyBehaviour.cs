using System;
using System.ComponentModel;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class LightBody : DisposableBattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public LightBody() {}
        private LightBody(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new LightBody(context, owner, state);
        }

        public void ReduceCost(TryUseCardBattleAction tryUseCard, BattleContext context)
        {
            OnExecuted();
            tryUseCard.ReduceCost(state.StackCount);

            RequestExpire();
        }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<TryUseCardBattleAction>(ReduceCost, PipelinePhaseStep.EARLY);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<TryUseCardBattleAction>(ReduceCost);
        }
    }
}