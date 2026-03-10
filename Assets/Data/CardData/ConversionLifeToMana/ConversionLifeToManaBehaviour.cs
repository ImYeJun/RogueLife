#nullable enable

using System;
using System.ComponentModel;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class ConversionLifeToMana : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ConversionLifeToMana() {}
        private ConversionLifeToMana(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new ConversionLifeToMana(owner, targetType, reflectionTargetType);
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
            ExecuteCommonAction(context, caster, target, 1f);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 0.5f);
        }
        private void ExecuteCommonAction(BattleContext context, CardCaster caster, PlayerCardTarget target, float damageMultiplier)
        {
            var usedCost = context.ActionCostHistory.GetConsumedActionCostCount(BattleScope.PHASE);
            int finalDamage = (int)(usedCost * damageMultiplier);

            var player = target.Player;
            var hurtAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), finalDamage, player);
            var fulfillCostAction = new FulfillActionCostBattleAction();
            
            context.ActionScheduler.Enqueue(hurtAction);
            context.ActionScheduler.Enqueue(fulfillCostAction);
        }
    }
}