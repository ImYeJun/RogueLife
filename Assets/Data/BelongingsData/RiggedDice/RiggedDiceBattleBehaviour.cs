using System;
using System.Collections.Generic;
using System.Linq;
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

            var outcomes = new List<(double weight, Action action)>
            {
                (20.0, () => 
                {
                    var requestRequestTriggerCard = new RequestTryTriggerCardBattleAction(card, false);
                    OnExecuted();
                    context.ActionScheduler.Enqueue(requestRequestTriggerCard);
                }),
                (15.0, () => 
                {
                    var hurtAction = new RequestHurtEntityBattleAction(new NoneEntitySource(), 15, context.PlayerContainer.Player);
                    OnExecuted();
                    context.ActionScheduler.Enqueue(hurtAction);
                }),
                (65.0, () => 
                {
                })
            };

            double totalWeight = outcomes.Sum(o => o.weight);

            double randomValue = context.Random.NextDouble() * totalWeight;

            double currentWeight = 0;
            foreach (var outcome in outcomes)
            {
                currentWeight += outcome.weight;
                if (randomValue <= currentWeight)
                {
                    outcome.action?.Invoke();
                    break;
                }
            }
        }
    }
}