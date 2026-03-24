using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class LuckyNumber : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public LuckyNumber() {}
        private LuckyNumber(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType)
        : base(owner, targetType, reflectionTargetType) { }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new LuckyNumber(owner, targetType, reflectionTargetType);
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
                context.HandDeck.GetCardsCountByCondition(CardRarity.ANY, CardAttribute.ANY, CardType.ANY) == 7 &&
                context.ActionCost.RemainCost == 7 &&
                (
                    context.GraveDeck.GetCardsCountByCondition(CardRarity.ANY, CardAttribute.ANY, CardType.ANY) == 7 ||
                    context.DrawDeck.GetCardsCountByCondition(CardRarity.ANY, CardAttribute.ANY, CardType.ANY) == 7
                );
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            var winAction = new RequestBattleEndBattleAction(BattleResultType.PLAYER_SPECIAL_CARD_WIN);
            context.ActionScheduler.Enqueue(winAction);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            var winAction = new RequestBattleEndBattleAction(BattleResultType.PLAYER_SPECIAL_CARD_WIN);
            context.ActionScheduler.Enqueue(winAction);
        }
    }
}