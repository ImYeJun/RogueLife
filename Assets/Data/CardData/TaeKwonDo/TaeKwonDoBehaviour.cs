using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class TaeKwonDo : CardBattleBehaviour<SingleEnemyCardTarget, SingleEnemyCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TaeKwonDo() {}
        private TaeKwonDo(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new TaeKwonDo(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, SingleEnemyCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, SingleEnemyCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 3);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 4);
        }
        private void ExecuteCommonAction(BattleContext context, CardCaster caster, SingleEnemyCardTarget target, int kickCount)
        {
            var enemy = target.Enemy;
            for (int i = 0; i < kickCount; i++)
            {
                var kickAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), 10, enemy);
                context.ActionScheduler.Enqueue(kickAction);
            }
        }
    }
}