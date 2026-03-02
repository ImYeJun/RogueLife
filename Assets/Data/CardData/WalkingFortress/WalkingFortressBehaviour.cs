using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class WalkingFortress : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectEntity defensiveStanceEntity;
        [SerializeField] BattleStatusEffectEntity weakenMuscleEntity;
        [SerializeField] BattleStatusEffectEntity counterAttackEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WalkingFortress() {}
        private WalkingFortress(ICardBehaviourOwner owner, BattleStatusEffectEntity defensiveStanceEntity, BattleStatusEffectEntity weakenMuscleEntity, BattleStatusEffectEntity counterAttackEntity)
        : base(owner)
        {
            this.defensiveStanceEntity = defensiveStanceEntity;
            this.weakenMuscleEntity = weakenMuscleEntity;
            this.counterAttackEntity = counterAttackEntity;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new WalkingFortress(owner, defensiveStanceEntity, weakenMuscleEntity, counterAttackEntity);
        }

        public override bool OnIsAbleToUse(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            var weakenMuscle = new BattleStatusEffect(weakenMuscleEntity, 1);
            var applyWeakenMuscle = new ApplyEntityStatusEffectBattleAction(target.Player, weakenMuscle);
            context.ActionScheduler.Enqueue(applyWeakenMuscle);

            ExecuteCommonAction(context, target);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            var counterAttack = new BattleStatusEffect(counterAttackEntity, 2);
            var applyCounterAttack = new ApplyEntityStatusEffectBattleAction(target.Player, counterAttack);
            context.ActionScheduler.Enqueue(applyCounterAttack);

            ExecuteCommonAction(context, target);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target)
        {
            var defensiveStance = new BattleStatusEffect(defensiveStanceEntity, 6);
            var applyDefensiveStance = new ApplyEntityStatusEffectBattleAction(target.Player, defensiveStance);
            context.ActionScheduler.Enqueue(applyDefensiveStance);
        }
    }
}