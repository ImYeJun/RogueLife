#nullable enable

using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class LifeRiskingBet : CardBattleBehaviour<CompositeCardTarget, CompositeCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public LifeRiskingBet() {}
        private LifeRiskingBet(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new LifeRiskingBet(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, CompositeCardTarget target)
        {
            return CheckCommonCondition(target, 30, 70);
        }
        public override bool OnIsAbleToUseReflect(BattleContext context, CompositeCardTarget target)
        {
            return CheckCommonCondition(target, 30, 100);
        }
        private bool CheckCommonCondition(CompositeCardTarget target, int minDifference, int maxDifference)
        {
            var player = target.GetTarget<PlayerCardTarget>()?.Player;
            var enemy = target.GetTarget<SingleEnemyCardTarget>()?.Enemy;
            if (player is null || enemy is null) { throw new InvalidOperationException("[LifeRiskingBet] Given type is not approriate"); }

            int difference = Mathf.Abs(player.CurrentHealth - enemy.CurrentHealth);
            return (minDifference <= difference) && (difference <= maxDifference);
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, CompositeCardTarget target)
        {
            ExecuteCommonAction(context, caster, target);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, CompositeCardTarget target)
        {
            ExecuteCommonAction(context, caster, target);
        }
        private void ExecuteCommonAction(BattleContext context, CardCaster caster, CompositeCardTarget target)
        {
            var player = target.GetTarget<PlayerCardTarget>()?.Player;
            var enemy = target.GetTarget<SingleEnemyCardTarget>()?.Enemy;
            if (player is null || enemy is null) { throw new InvalidOperationException("[LifeRiskingBet] Given type is not approriate"); }

            int difference = Mathf.Abs(player.CurrentHealth - enemy.CurrentHealth);

            if (context.Random.Next(100) < difference)
            {
                var hurtEnemyAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), difference * 2, enemy);
                context.ActionScheduler.Enqueue(hurtEnemyAction);
            }
            else
            {
                var hurtPlayerAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), difference / 2, player);
                context.ActionScheduler.Enqueue(hurtPlayerAction);
            }
        }
    }
}