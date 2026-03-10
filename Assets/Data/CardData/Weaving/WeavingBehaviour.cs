using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class Weaving : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectEntity tooSlowEntity;
        [SerializeField] BattleStatusEffectEntity strengthenMuscleEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Weaving() {}
        private Weaving(ICardBehaviourOwner owner, BattleStatusEffectEntity tooSlowEntity, BattleStatusEffectEntity strengthenMuscleEntity,CardTargetType targetType, CardTargetType reflectionTargetType
)
        : base(owner, targetType, reflectionTargetType)
        {
            this.tooSlowEntity = tooSlowEntity;
            this.strengthenMuscleEntity = strengthenMuscleEntity;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new Weaving(owner, tooSlowEntity, strengthenMuscleEntity, targetType, reflectionTargetType);
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
            ExecuteCommonAction(context, target);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target);

            var player = target.Player;
            var strengthenMuscle = new BattleStatusEffect(strengthenMuscleEntity, 2, 2);
            var applyBuffAction = new ApplyEntityStatusEffectBattleAction(player, strengthenMuscle);
            context.ActionScheduler.Enqueue(applyBuffAction);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target)
        {
            var player = target.Player;
            var tooSlow = new BattleStatusEffect(tooSlowEntity, 1, 2);
            var applyBuffAction = new ApplyEntityStatusEffectBattleAction(player, tooSlow);
            context.ActionScheduler.Enqueue(applyBuffAction);
        }
    }
}