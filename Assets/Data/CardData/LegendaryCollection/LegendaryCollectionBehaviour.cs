using System;
using System.ComponentModel;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class LegendaryCollectionBehaviour : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public LegendaryCollectionBehaviour() {}
        private LegendaryCollectionBehaviour(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new LegendaryCollectionBehaviour(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, NoneCardTarget target)
        {
            return CheckCommonCondition(context);
        }
        public override bool OnIsAbleToUseReflect(BattleContext context, NoneCardTarget target)
        {
            return CheckCommonCondition(context);
        }
        private bool CheckCommonCondition(BattleContext context)
        {
            return
                context.HandDeck.GetCardsCountByCondition(CardRarity.ANY, CardAttribute.LUCK, CardType.SPECIAL) == 3 &&
                context.HandDeck.GetCardsCountByCondition(CardRarity.ANY, CardAttribute.MAGIC, CardType.EFFECT) == 3 &&
                context.HandDeck.GetCardsCountByCondition(CardRarity.ANY, CardAttribute.PHYSICAL, CardType.ATTACK) == 3;
        }

        public override void OnDraw(BattleContext context)
        {
            return;
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
            var action = new RequestBattleEndBattleAction(BattleResultType.PLAYER_SPECIAL_CARD_WIN);
            context.ActionScheduler.EnqueueFront(action);
        }
    }
}