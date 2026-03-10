using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class FierceMomentum : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] private BattleStatusEffectEntity strengthenMuscleEntity;
        [SerializeField] private BattleStatusEffectEntity iWillKillYouEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FierceMomentum() {}
        private FierceMomentum(ICardBehaviourOwner owner, BattleStatusEffectEntity strengthenMuscleEntity, BattleStatusEffectEntity iWillKillYouEntity, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType)
        {
            this.strengthenMuscleEntity = strengthenMuscleEntity;
            this.iWillKillYouEntity = iWillKillYouEntity;
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new FierceMomentum(owner, strengthenMuscleEntity, iWillKillYouEntity, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }
        public override bool OnIsAbleToUseReflect(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, strengthenMuscleEntity);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, iWillKillYouEntity);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, BattleStatusEffectEntity statusEffectEntity)
        {
            var statusEffect = new BattleStatusEffect(statusEffectEntity, 3, 2);
            var applyStatusEffectAction = new ApplyEntityStatusEffectBattleAction(target.Player, statusEffect);
            context.ActionScheduler.Enqueue(applyStatusEffectAction);
        }
    }
}