using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class ToxicBombShot : CardBattleBehaviour<AllEnemyCardTarget, AllEnemyCardTarget>
    {
        [SerializeField] private BattleStatusEffectEntity deadlyPoisionEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ToxicBombShot() {}
        private ToxicBombShot(ICardBehaviourOwner owner, BattleStatusEffectEntity deadlyPoisionEntity, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType)
        {
            this.deadlyPoisionEntity = deadlyPoisionEntity;
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new ToxicBombShot(owner, deadlyPoisionEntity, targetType, reflectionTargetType);
        }
        
        public override bool OnIsAbleToUse(BattleContext context, AllEnemyCardTarget target)
        {
            return true;
        }
        public override bool OnIsAbleToUseReflect(BattleContext context, AllEnemyCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, AllEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 15, 3, 2);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, AllEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 20, 4, 3);
        }
        private void ExecuteCommonAction(BattleContext context, CardCaster caster, AllEnemyCardTarget target, int damage, int debuffDuration, int debuffStack)
        {
            foreach (var enemy in target.Enemies)
            {
                var hurtAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), damage, enemy);
                context.ActionScheduler.Enqueue(hurtAction);
            }
            foreach (var enemy in target.Enemies)
            {
                var statusEffect = new BattleStatusEffect(deadlyPoisionEntity, debuffStack, debuffDuration);
                var debuffAction = new ApplyEntityStatusEffectBattleAction(enemy, statusEffect);
                context.ActionScheduler.Enqueue(debuffAction);
            }
        }
    }
}