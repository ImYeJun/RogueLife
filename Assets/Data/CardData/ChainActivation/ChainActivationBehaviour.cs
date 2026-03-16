#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class ChainActivation : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ChainActivation() {}
        private ChainActivation(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new ChainActivation(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, NoneCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, NoneCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, 0.4);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, 0.5);
        }
        private void ExecuteCommonAction(BattleContext context, double probability)
        {
            int totalExecuteCount = 0;
            
            while (true)
            {
                if (context.Random.NextDouble() <= probability) { totalExecuteCount++; }
                else { break; }
            }

            var availableCards = context.HandDeck.GetCards().Where(card => card != owner).ToList();
            if (availableCards.Count == 0 || totalExecuteCount == 0) { return; }

            List<Card> cardsToTrigger = new List<Card>();
            for (int i = 0; i < totalExecuteCount; i++)
            {
                cardsToTrigger.Add(availableCards[context.Random.Next(availableCards.Count)]);
            }

            context.ActionScheduler.Enqueue(new SequentialCardTriggerRequestBattleAction(cardsToTrigger, true));
        }
    }
}