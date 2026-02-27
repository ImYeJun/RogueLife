using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class BattlePrettyHeartMagicWand : BattleBelongingsBehaviour
    {
        public override BattleBelongingsBehaviour Clone()
        {
            return new BattlePrettyHeartMagicWand();
        }

        protected override void OnApplied()
        {
            context.ActionObserverHub.SubscribePreObserver<UseCardBattleAction>(PreUseCard);
        }

        protected override void OnRemoved()
        {
            context.ActionObserverHub.UnsubscribePreObserver<UseCardBattleAction>(PreUseCard);
        }

        public void PreUseCard(UseCardBattleAction useCard, BattleContext context)
        {
            var card = useCard.Card;
            if (card.CurrentAttribute != CardAttribute.MAGIC || !card.IsReflectionApplied) { return; }
            
            var pivot = context.Random.NextDouble();
            if (pivot <= 0.1)
            {
                var restoreCostAction = new RestoreActionCostBattleAction(1);

                context.ActionScheduler.Enqueue(restoreCostAction);
            }
        }
    }
}