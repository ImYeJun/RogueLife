using System;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class MatchBrokenClock : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MatchBrokenClock() {}
        private MatchBrokenClock(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType)
        : base(owner, targetType, reflectionTargetType) { }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new MatchBrokenClock(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, NoneCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, NoneCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, 0.3, 1);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, 0.4, 2);
        }
        private void ExecuteCommonAction(BattleContext context, double successProbability, int selectCardCount)
        {
            var random = context.Random;
            var filteredHandCards = context.HandDeck.GetCards().Where(card => card != owner).ToList();
            
            var cardCount = filteredHandCards.Count;
            if (cardCount <= 0) { return; }

            if (context.Random.NextDouble() <= successProbability)
            {
                var selectedCards = filteredHandCards.OrderBy(card => random.Next()).Take(selectCardCount);

                foreach (var card in selectedCards)
                {
                    var applyReflectAction = new ApplyReflectEffectOnCard(card);
                    context.ActionScheduler.Enqueue(applyReflectAction);
                }
            }
            else
            {
                var selectedCard = filteredHandCards[random.Next(cardCount)];

                var moveCardAction = new MoveCardToDeckBattleAction(selectedCard, BattleDeckType.GRAVE);
                context.ActionScheduler.Enqueue(moveCardAction);
            }
        }
    }
}