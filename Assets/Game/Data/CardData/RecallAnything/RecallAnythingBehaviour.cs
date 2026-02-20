using System;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class RecallAnything : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RecallAnything() {}
        private RecallAnything(ICardBehaviourOwner owner) 
        : base(owner) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new RecallAnything(owner);
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
            ExecuteCommonAction(context, 1);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, 2);
        }

        private static void ExecuteCommonAction(BattleContext context, int count)
        {
            var random = context.Random;
            var handCards = context.HandDeck.GetCards();

            var selectedCards = handCards.Where(card => !card.IsReflectionApplied).OrderBy(card => random.Next()).Take(count);
            foreach (var selectedCard in selectedCards)
            {
                var action = new ApplyReflectEffectOnCard(selectedCard);
                context.ActionScheduler.Enqueue(action);
            }
        }
    }
}