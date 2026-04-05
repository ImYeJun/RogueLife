using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class BattleCarrotCarrot : BattleBelongingsBehaviour
    {
        public override BattleBelongingsBehaviour Clone()
        {
            return new BattleCarrotCarrot();
        }

        protected override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<HurtPlayerBattleAction>(OnPlayerHurt, PipelinePhaseStep.LATE);
        }

        protected override void OnRemoved()
        {
            context.ActionObserverHub.UnsubscribeActionModifier<HurtPlayerBattleAction>(OnPlayerHurt);
        }

        public void OnPlayerHurt(HurtPlayerBattleAction hurtPlayer, BattleContext context)
        {
            if (hurtPlayer.MentalityDamage <= 0) { return; }

            OnExecuted();
            hurtPlayer.NullifyMentalityDamage();

            Deactivate();
        }
    }
}