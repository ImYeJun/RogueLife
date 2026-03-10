using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class UltimateExplosion : CardBattleBehaviour<AllEnemyCardTarget, AllEnemyCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public UltimateExplosion() {}
        private UltimateExplosion(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType)
        : base(owner, targetType, reflectionTargetType) {}
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new UltimateExplosion(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, AllEnemyCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, AllEnemyCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, AllEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 5);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, AllEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 10);
        }
        private void ExecuteCommonAction(BattleContext context, CardCaster caster, AllEnemyCardTarget target, int baseDamage)
        {
            int remainCost = context.ActionCost.RemainCost;
            var useAllCostAction = new ConsumeActionCostBattleAction(remainCost);
            context.ActionScheduler.Enqueue(useAllCostAction);

            foreach (var enemy in target.Enemies)
            {
                var hurtEnemyAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), remainCost * baseDamage, enemy);
                context.ActionScheduler.Enqueue(hurtEnemyAction);
            }

            context.ActionScheduler.Enqueue(new RequestPlayerTurnEndBattleAction());
        }
    }
}