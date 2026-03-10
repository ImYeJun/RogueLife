#nullable enable

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class ManaOverload : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ManaOverload() {}
        private ManaOverload(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new ManaOverload(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, NoneCardTarget target)
        {
            return context.ActionCostHistory.GetConsumedActionCostCount(BattleScope.BATTLE) >= 20; 
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, NoneCardTarget target)
        {
            return context.ActionCostHistory.GetConsumedActionCostCount(BattleScope.BATTLE) >= 20;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, 5);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, 7);
        }
        private void ExecuteCommonAction(BattleContext context, int restoreCostAmount)
        {
            var restoreCostAction = new RestoreActionCostBattleAction(restoreCostAmount);
            context.ActionScheduler.Enqueue(restoreCostAction);

            var handCards = context.HandDeck.GetCards();
            for (int i = handCards.Count - 1; i >= 0; i--)
            {
                var decreaseCardCostAction = new DecreaseCardActionCost(handCards[i], 2, BattleScope.TURN);
                context.ActionScheduler.Enqueue(decreaseCardCostAction);
            }
        }
    }
}