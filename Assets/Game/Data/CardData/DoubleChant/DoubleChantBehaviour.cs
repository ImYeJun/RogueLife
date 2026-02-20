#nullable enable

using System;
using System.ComponentModel;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class DoubleChant : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DoubleChant() {}
        private DoubleChant(ICardBehaviourOwner owner) 
        : base(owner) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new DoubleChant(owner);
        }

        public override bool OnIsAbleToUse(BattleContext context, NoneCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, NoneCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context);

            var restoreCostAction = new RestoreActionCostBattleAction(1);
            context.ActionScheduler.Enqueue(restoreCostAction);
        }
        private void ExecuteCommonAction(BattleContext context)
        {
            Card? previousUsedCard = context.BattleDeckHistory.GetRecentlyPlayedCard(owner);
            if (previousUsedCard is null) { return; }

            var triggerCardEffectAction = new TryTriggerCardEffectBattleAction(previousUsedCard, null);
            //TODO Delegate the TriggerCardBattleAction action to UI
            UnityEngine.Debug.LogError("[DoubleChant] card target should be chosen in the UI. Delegate it!");
            // context.ActionScheduler.Enqueue(triggerCardEffectAction);
        }
    }
}