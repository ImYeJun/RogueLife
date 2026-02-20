using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class WalkingFortress : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectData defensiveStanceData;
        [SerializeField] BattleStatusEffectData weakenMuscleData;
        [SerializeField] BattleStatusEffectData counterAttackData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WalkingFortress() {}
        private WalkingFortress(ICardBehaviourOwner owner, BattleStatusEffectData defensiveStanceData, BattleStatusEffectData weakenMuscleData, BattleStatusEffectData counterAttackData)
        : base(owner)
        {
            this.defensiveStanceData = defensiveStanceData;
            this.weakenMuscleData = weakenMuscleData;
            this.counterAttackData = counterAttackData;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new WalkingFortress(owner, defensiveStanceData, weakenMuscleData, counterAttackData);
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
            var weakenMuscle = new BattleStatusEffect(weakenMuscleData, 1);
            var applyWeakenMuscle = new ApplyEntityStatusEffectBattleAction(target.Player, weakenMuscle);
            context.ActionScheduler.Enqueue(applyWeakenMuscle);

            ExecuteCommonAction(context, target);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            var counterAttack = new BattleStatusEffect(counterAttackData, 2);
            var applyCounterAttack = new ApplyEntityStatusEffectBattleAction(target.Player, counterAttack);
            context.ActionScheduler.Enqueue(applyCounterAttack);

            ExecuteCommonAction(context, target);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target)
        {
            var defensiveStance = new BattleStatusEffect(defensiveStanceData, 6);
            var applyDefensiveStance = new ApplyEntityStatusEffectBattleAction(target.Player, defensiveStance);
            context.ActionScheduler.Enqueue(applyDefensiveStance);
        }
    }
}