using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class UnknownKnowledge : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public UnknownKnowledge() {}
        private UnknownKnowledge(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType)
        : base(owner, targetType, reflectionTargetType) {}
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new UnknownKnowledge(owner, targetType, reflectionTargetType);
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
            ExecuteCommonAction(context, CardRarity.COMMON, CardRarity.RARE);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, CardRarity.RARE, CardRarity.RARE);
        }
        private void ExecuteCommonAction(BattleContext context, CardRarity lowestRarity, CardRarity highestRarity)
        {
            var handMagicCards = context.HandDeck.GetCardsByCondition(CardRarity.ANY, CardAttribute.MAGIC, CardType.ANY).Select(card => card.Data).ToList();
            var selectedCard = context.CardDatabase.GetRandomCard(context.Random, lowestRarity, highestRarity, CardType.ANY, CardAttribute.MAGIC, handMagicCards);

            if (selectedCard is null) { return; }

            var requestTryTriggerCardEffect = new RequestTryTriggerCardBattleAction(selectedCard, true);
            context.ActionScheduler.Enqueue(requestTryTriggerCardEffect);
        }
    }
}