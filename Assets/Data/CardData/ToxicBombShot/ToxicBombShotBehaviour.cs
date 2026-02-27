using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class ToxicBombShot : CardBattleBehaviour<AllEnemyCardTarget, AllEnemyCardTarget>
    {
        [SerializeField] private BattleStatusEffectData deadlyPoisionData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ToxicBombShot() {}
        private ToxicBombShot(ICardBehaviourOwner owner, BattleStatusEffectData deadlyPoisionData) 
        : base(owner)
        {
            this.deadlyPoisionData = deadlyPoisionData;
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new ToxicBombShot(owner, deadlyPoisionData);
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
            ExecuteCommonAction(context, caster, target, 2);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, AllEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 3);
        }
        private void ExecuteCommonAction(BattleContext context, CardCaster caster, AllEnemyCardTarget target, int debuffDuration)
        {
            foreach (var enemy in target.Enemies)
            {
                var hurtAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), 10, enemy);
                context.ActionScheduler.Enqueue(hurtAction);
            }
            foreach (var enemy in target.Enemies)
            {
                var statusEffect = new BattleStatusEffect(deadlyPoisionData, 2, debuffDuration);
                var debuffAction = new ApplyEntityStatusEffectBattleAction(enemy, statusEffect);
                context.ActionScheduler.Enqueue(debuffAction);
            }
        }
    }
}