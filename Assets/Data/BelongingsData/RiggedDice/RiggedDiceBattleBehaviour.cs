using System;
using Battle.HurtSources;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class BattleRiggedDice : BattleBelongingsBehaviour
    {
        public override BattleBelongingsBehaviour Clone()
        {
            return new BattleRiggedDice();
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
            if (card.CurrentAttribute != CardAttribute.LUCK) { return; }
            
            var pivot = context.Random.NextDouble();
            if (pivot <= 0.05)
            {
                var requestRequestTriggerCard = new RequestTryTriggerCardBattleAction(card, false);

                OnExecuted();
                context.ActionScheduler.Enqueue(requestRequestTriggerCard);
            }
            else if (pivot <= 0.2)
            {
                var hurtAction = new RequestHurtEntityBattleAction(new NoneEntitySource(), 20, context.PlayerContainer.Player);

                OnExecuted();
                context.ActionScheduler.Enqueue(hurtAction);
            }
        }
    }
}