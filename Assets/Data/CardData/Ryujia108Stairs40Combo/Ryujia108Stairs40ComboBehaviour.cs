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
            foreach (var card in cards)
            {
                var requestTryUseCardAction = new RequestTryUseCardBattleAction(card, true);
                context.ActionScheduler.Enqueue(requestTryUseCardAction);
            }
        }
    }
}