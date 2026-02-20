using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class WhyAmITheOnlyOneToHurt : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectData holyShieldData;
        [SerializeField] BattleStatusEffectData nanoMachineData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WhyAmITheOnlyOneToHurt() {}
        private WhyAmITheOnlyOneToHurt(ICardBehaviourOwner owner, BattleStatusEffectData holyShieldData, BattleStatusEffectData nanoMachineData)
        : base(owner)
        {
            this.holyShieldData = holyShieldData;
            this.nanoMachineData = nanoMachineData;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new WhyAmITheOnlyOneToHurt(owner, holyShieldData, nanoMachineData);
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

            var nanoMachine = new BattleStatusEffect(nanoMachineData, 1, 2);
            var nanoMachinApplyAction = new ApplyEntityStatusEffectBattleAction(target.Player, nanoMachine);
            context.ActionScheduler.Enqueue(nanoMachinApplyAction);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target)
        {
            var holyShield = new BattleStatusEffect(holyShieldData, 2, 2);
            var holyShieldApplyAction = new ApplyEntityStatusEffectBattleAction(target.Player, holyShield);
            context.ActionScheduler.Enqueue(holyShieldApplyAction);
        }
    }
}