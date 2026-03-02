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
        private Fireball(ICardBehaviourOwner owner, BattleStatusEffectEntity burningEntity) 
        : base(owner)
        {
            this.burningEntity = burningEntity;
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new Fireball(owner, burningEntity);
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
            ExecuteCommonAction(context, caster, target);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target);

            if (owner.CurrentActionCost <= 0) { return; }

            var decreaseCardActionCostAcion = new DecreaseCardActionCost(owner, 1, BattleScope.BATTLE);
            context.ActionScheduler.Enqueue(decreaseCardActionCostAcion);
        }
        
        private void ExecuteCommonAction(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            var targetEnemy = target.Enemy;

            var hurtAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), 10, targetEnemy);

            var itsFire = new BattleStatusEffect(burningEntity, 1, 1);
            var debuffApplyAction = new ApplyEntityStatusEffectBattleAction(targetEnemy, itsFire);

            context.ActionScheduler.Enqueue(hurtAction);
            context.ActionScheduler.Enqueue(debuffApplyAction);
        }
    }
}