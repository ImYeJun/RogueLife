using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class ManaRecovery : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ManaRecovery() {}
        private ManaRecovery(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType)
        : base(owner, targetType, reflectionTargetType) { }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new ManaRecovery(owner, targetType, reflectionTargetType);
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
            ExecuteCommonAction(context);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context);

            var drawCardAction = new RequestDrawCardBattleAction(Guid.NewGuid());
            context.ActionScheduler.Enqueue(drawCardAction);
        }
        private void ExecuteCommonAction(BattleContext context)
        {
            var recentHistory = context.BattleDeckHistory.GetRecentlyPlayedHistory(owner);
            if (recentHistory is null) { return; }
            var previousUsedCard = recentHistory.Value.UsedCard;
            
            var restoreCostAction = new RestoreActionCostBattleAction(previousUsedCard.CurrentActionCost);
            var moveCardAction = new MoveCardToDeckBattleAction(previousUsedCard, BattleDeckType.DRAW);

            context.ActionScheduler.Enqueue(restoreCostAction);
            context.ActionScheduler.Enqueue(moveCardAction);
        }
    }
}