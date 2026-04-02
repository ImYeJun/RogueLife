using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class Fireball : CardBattleBehaviour<SingleEnemyCardTarget, SingleEnemyCardTarget>
    {
        [SerializeField] private BattleStatusEffectEntity burningEntity;
        
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Fireball() {}
        private Fireball(ICardBehaviourOwner owner, BattleStatusEffectEntity burningEntity, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType)
        {
            this.burningEntity = burningEntity;
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new Fireball(owner, burningEntity, targetType, reflectionTargetType);
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
            ExecuteCommonAction(context, caster, target, 20, 2);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 25, 3);

            if (owner.CurrentActionCost <= 0) { return; }

            var mod = new CardCostModifier(-1);
            var decreaseCardActionCostAcion = new AddCardCostModifierBattleAction((Card)owner, mod);
            context.ActionScheduler.Enqueue(decreaseCardActionCostAcion);
        }
        
        private void ExecuteCommonAction(BattleContext context, CardCaster caster, SingleEnemyCardTarget target, int damage, int duration)
        {
            var targetEnemy = target.Enemy;

            var hurtAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), damage, targetEnemy);

            var itsFire = new BattleStatusEffect(burningEntity, 1, duration);
            var debuffApplyAction = new ApplyEntityStatusEffectBattleAction(targetEnemy, itsFire);

            context.ActionScheduler.Enqueue(hurtAction);
            context.ActionScheduler.Enqueue(debuffApplyAction);
        }
    }
}