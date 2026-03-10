using System;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class AllOutAttack : CardBattleBehaviour<SingleEnemyCardTarget, SingleEnemyCardTarget>
    {

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AllOutAttack() {}
        private AllOutAttack(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType)
        : base(owner, targetType, reflectionTargetType) { }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new AllOutAttack(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, SingleEnemyCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, SingleEnemyCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            ExecuteCommon(context, caster, target, 2);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            ExecuteCommon(context, caster, target, 3);
        }

        private void ExecuteCommon(BattleContext context, CardCaster caster, SingleEnemyCardTarget target, int decreaseCardCostAmount)
        {
            var hurtEnemyAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), 10, target.Enemy);
            context.ActionScheduler.Enqueue(hurtEnemyAction);

            var physicalCards = context.HandDeck.GetCardsByCondition(CardRarity.ANY, CardAttribute.PHYSICAL, CardType.ATTACK);
            var filteredCards = physicalCards.Where(card => card != owner).ToList();
            for (int i = filteredCards.Count - 1; i >= 0; i--)
            {
                var decreaseCardCostAction = new DecreaseCardActionCost(filteredCards[i], decreaseCardCostAmount, BattleScope.BATTLE);
                context.ActionScheduler.Enqueue(decreaseCardCostAction);
            }
        }
    }
}