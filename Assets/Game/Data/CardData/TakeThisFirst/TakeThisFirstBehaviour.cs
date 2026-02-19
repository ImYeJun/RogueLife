using System;
using Battle.Cards.Casters;
using Battle.HurtSources;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class TakeThisFirst : CardBattleBehaviour<SingleEnemyCardTarget, SingleEnemyCardTarget>
    {
        public override CardBattleBehaviour Clone()
        {
            return new TakeThisFirst();
        }

        public override bool IsAbleToUse(BattleContext context, CardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            var hurtSource = caster.GetAsHurtSource();
            int damage = 20;
            var targetEntity = target.Enemy;

            var action = new RequestHurtEntityBattleAction(hurtSource, damage, targetEntity);
            context.ActionScheduler.Enqueue(action);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            var hurtSource = caster.GetAsHurtSource();
            int damage = 30;
            var targetEntity = target.Enemy;

            var action = new RequestHurtEntityBattleAction(hurtSource, damage, targetEntity);
            context.ActionScheduler.Enqueue(action);
        }
    }
}