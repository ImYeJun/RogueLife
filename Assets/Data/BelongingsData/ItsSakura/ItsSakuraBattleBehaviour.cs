using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class BattleItsSakura : BattleBelongingsBehaviour
    {
        private int remainOpportunity;

        public override BattleBelongingsBehaviour Clone()
        {
            return new BattleItsSakura();
        }

        protected override void OnApplied()
        {
            context.EventBus.Subscribe<PlayerTurnStartBattleEvent>(OnPlayerTurnStart, BattleEventObserverStage.PRE);
            context.ActionObserverHub.SubscribePostObserver<DrawCardBattleAction>(PostDrawCard);
        }

        protected override void OnRemoved()
        {
            context.EventBus.Unsubscribe<PlayerTurnStartBattleEvent>(OnPlayerTurnStart);
            context.ActionObserverHub.UnsubscribePostObserver<DrawCardBattleAction>(PostDrawCard);
        }

        public void OnPlayerTurnStart(PlayerTurnStartBattleEvent payload)
        {
            remainOpportunity = 2;
        }

        public void PostDrawCard(DrawCardBattleAction drawCard, BattleContext context)
        {
            if (remainOpportunity <= 0) { return; }

            if (context.Random.NextDouble() <= 0.2)
            {
                remainOpportunity--;

                var requestDrawCardAction = new RequestDrawCardBattleAction(Guid.NewGuid());
                
                OnExecuted();
                context.ActionScheduler.Enqueue(requestDrawCardAction);
            }
        }
    }
}