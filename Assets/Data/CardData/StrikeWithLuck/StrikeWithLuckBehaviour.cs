using System;
using System.ComponentModel;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class StrikeWithLuck : CardBattleBehaviour<SingleEnemyCardTarget, SingleEnemyCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public StrikeWithLuck() {}
        private StrikeWithLuck(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new StrikeWithLuck(owner, targetType, reflectionTargetType);
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
            ExecuteCommonAction(context, caster, target, 0.3);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 0.4);
        }
        private void ExecuteCommonAction(BattleContext context, CardCaster caster, SingleEnemyCardTarget target, double probability)
        {
            var targetEnemy = target.Enemy;

            var firstHurtAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), 10, targetEnemy);
            context.ActionScheduler.Enqueue(firstHurtAction);

            if (context.Random.NextDouble() > probability) { return; }

            var secondHurtAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), 30, targetEnemy);
            context.ActionScheduler.Enqueue(secondHurtAction);
        }
    }
}