using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class WhyAmITheOnlyOneToHurt : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectEntity holyShieldEntity;
        [SerializeField] BattleStatusEffectEntity nanoMachineEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WhyAmITheOnlyOneToHurt() {}
        private WhyAmITheOnlyOneToHurt(ICardBehaviourOwner owner, BattleStatusEffectEntity holyShieldEntity, BattleStatusEffectEntity nanoMachineEntity,CardTargetType targetType, CardTargetType reflectionTargetType
)
        : base(owner, targetType, reflectionTargetType)
        {
            this.holyShieldEntity = holyShieldEntity;
            this.nanoMachineEntity = nanoMachineEntity;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new WhyAmITheOnlyOneToHurt(owner, holyShieldEntity, nanoMachineEntity, targetType, reflectionTargetType);
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

            var nanoMachine = new BattleStatusEffect(nanoMachineEntity, 1, 2);
            var nanoMachinApplyAction = new ApplyEntityStatusEffectBattleAction(target.Player, nanoMachine);
            context.ActionScheduler.Enqueue(nanoMachinApplyAction);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target)
        {
            var holyShield = new BattleStatusEffect(holyShieldEntity, 2, 2);
            var holyShieldApplyAction = new ApplyEntityStatusEffectBattleAction(target.Player, holyShield);
            context.ActionScheduler.Enqueue(holyShieldApplyAction);
        }
    }
}