using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class Ryujia108Stairs40Combo : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Ryujia108Stairs40Combo() {}
        private Ryujia108Stairs40Combo(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType)
        : base(owner, targetType, reflectionTargetType) {}
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new Ryujia108Stairs40Combo(owner, targetType, reflectionTargetType);
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
            var cards = context.HandDeck.GetCardsByCondition(CardRarity.ANY, CardAttribute.PHYSICAL, CardType.ANY)
                                .Where(card => card != owner)
                                .ToList();
            ExecuteCommonAction(context, cards);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            var cards = context.HandDeck.GetCards()
                        .Where(card => card != owner)
                        .ToList();
            ExecuteCommonAction(context, cards);
        }

        private void ExecuteCommonAction(BattleContext context, List<Card> cards)
        {
            // 💡 [수정된 부분]
            // 기존의 foreach 문을 완전히 삭제하고, 우리가 만든 "순차 발동 매크로 액션" 하나로 교체!
            // 이전 코드에서 isFreeUse를 true로 넘겼으므로, 매크로 액션에도 true를 명시적으로 넘겨준다.
            if (cards.Count > 0)
            {
                context.ActionScheduler.Enqueue(new SequentialCardUseRequestBattleAction(cards, true));
            }
        }
    }
}