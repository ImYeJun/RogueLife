#nullable enable

using System;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class DoubleChant : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        public override CardBattleBehaviour Clone()
        {
            return new DoubleChant();
        }

        public override bool IsAbleToUse(BattleContext context, CardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            Card? previousUsedCard = context.BattleDeckHistory.GetRecentlyPlayedCard();
            if (previousUsedCard is null) { return; }

            // var action = new TryTriggerCardEffectBattleAction();
            // context.ActionScheduler.Enqueue()
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            throw new NotImplementedException();
        }
    }
}