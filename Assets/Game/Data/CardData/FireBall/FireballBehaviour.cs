using System;
using Battle.Cards.Casters;
using UnityEngine;

namespace  Battle.Cards.Behaviours
{
    [Serializable]
    public class Fireball : CardBattleBehaviour<SingleEnemyCardTarget, SingleEnemyCardTarget>
    {
        [SerializeField] private BattleStatusEffectData burningData;
        
        public override CardBattleBehaviour Clone()
        {
            return new Fireball();
        }

        public override bool IsAbleToUse(BattleContext context, CardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            
            
            ExecuteCommonAction(context, caster, target);
        }
        
        private void ExecuteCommonAction(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            var targetEnemy = target.Enemy;

            var hurtAction = new RequestHurtEntityBattleAction(caster.GetAsHurtSource(), 10, targetEnemy);

            var itsFire = new BattleStatusEffect(burningData, 1, 1);
            var debuffApplyAction = new ApplyEntityStatusEffectBattleAction(targetEnemy, itsFire);

            context.ActionScheduler.Enqueue(hurtAction);
            context.ActionScheduler.Enqueue(debuffApplyAction);
        }
    }
}