using System;
using System.ComponentModel;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class QuickEscape : BattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public QuickEscape() {}
        private QuickEscape(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }
        
        public override void OnApplied()
        {
            TryDecreasePhase();
        }

        public override void OnRemoved(bool isOwnerDied = false) { }

        public override void OnMerged()
        {
            TryDecreasePhase();
        }

        private void TryDecreasePhase()
        {
            if (state.StackCount < 3) { return; }

            OnExecuted();
            context.ActionScheduler.Enqueue(new DecreasePhaseCountBattleAction(1));
            state.RequestExpired();
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new QuickEscape(context, owner, state);
        }
    }
}