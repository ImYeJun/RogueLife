using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using Battle.HurtSources;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class ICanDoThisAllDay : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICanDoThisAllDay() {}
        private ICanDoThisAllDay(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new ICanDoThisAllDay(owner, targetType, reflectionTargetType);
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
            ExecuteCommonAction(context);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context);
            
            var healCostAction = new RestoreActionCostBattleAction(1);
            context.ActionScheduler.Enqueue(healCostAction);
        }
        private void ExecuteCommonAction(BattleContext context)
        {
            var cards = context.BattleDeckHistory.GetRecentlyGravedCard(3);
            foreach (var card in cards)
            {
                if (!context.DrawDeck.HasCard(card))
                {
                    var restoringAction = new MoveCardToDeckBattleAction(card, BattleDeckType.DRAW);
                    context.ActionScheduler.Enqueue(restoringAction);
                }
            }

            var drawAction = new RequestDrawCardBattleAction(CardRarity.ANY, CardAttribute.ANY, CardType.ANY, Guid.NewGuid());
            context.ActionScheduler.Enqueue(drawAction);
        }
    }
}