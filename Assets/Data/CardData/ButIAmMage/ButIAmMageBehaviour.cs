using System;
using System.ComponentModel;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class ButIAmMage : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ButIAmMage() {}
        private ButIAmMage(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new ButIAmMage(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, NoneCardTarget target)
        {
            return context.HandDeck.GetCardsCountByCondition(CardRarity.ANY, CardAttribute.MAGIC, CardType.ANY) == 0;
        }
        public override bool OnIsAbleToUseReflect(BattleContext context, NoneCardTarget target)
        {
            return !context.BattleDeckHistory.HasPlayedCard(CardRarity.ANY, CardAttribute.MAGIC, CardType.ANY, BattleScope.PHASE);
        }

        public override void OnDraw(BattleContext context)
        {
            
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context);
        }
        private static void ExecuteCommonAction(BattleContext context)
        {
            for (int i = 0; i < 3; i++)
            {
                var action = new RequestDrawCardBattleAction(CardRarity.ANY, CardAttribute.MAGIC, CardType.ANY, Guid.NewGuid());

                context.ActionScheduler.Enqueue(action);
            }
        }
    }
}