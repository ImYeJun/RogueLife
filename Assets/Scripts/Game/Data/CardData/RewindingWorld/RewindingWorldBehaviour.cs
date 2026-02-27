#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class RewindingWorld : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        private Observer? observer = null;
        private class Observer
        {
            private BattleContext context;
            private ICardBehaviourOwner owner;
            private List<CardCostModifier> costModifiers;

            public Observer(BattleContext context, ICardBehaviourOwner owner)
            {
                this.context = context;
                this.owner = owner;
                costModifiers = new List<CardCostModifier>();
            }

            public void PostUseCardEffect(UseCardEffectBattleAction useCardEffect, BattleContext context)
            {
                if (useCardEffect.Card.CurrentAttribute != CardAttribute.MAGIC) { return; }

                int baseActionCost = useCardEffect.Card.BaseActionCost;
                var costModifier = new CardCostModifier(-baseActionCost);
                
                owner.AddCostModifier(costModifier);
                costModifiers.Add(costModifier);
            }

            public void OnBattleEnd(BattleEndBattleEvent payload)
            {
                Clean();
            }

            public void Clean()
            {
                foreach (var costModifier in costModifiers)
                {
                    owner.RemoveCostModifier(costModifier);
                }

                context.ActionObserverHub.UnsubscribePostObserver<UseCardEffectBattleAction>(PostUseCardEffect);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RewindingWorld() {}
        private RewindingWorld(ICardBehaviourOwner owner) 
        : base(owner) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new RewindingWorld(owner);
        }

        public override bool OnIsAbleToUse(BattleContext context, NoneCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, NoneCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
            if (observer is not null) { return; }

            observer = new Observer(context, owner);

            context.ActionObserverHub.SubscribePostObserver<UseCardEffectBattleAction>(observer.PostUseCardEffect);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context);
        }
        private void ExecuteCommonAction(BattleContext context)
        {
            var currentTurnHistory = context.BattleDeckHistory.GetRecentPhasePlayedHistory(owner);
            if (currentTurnHistory is null) { return; }

            var magicCardsHistory = currentTurnHistory
                                    .Where(history => history.UsedCard.CurrentAttribute == CardAttribute.MAGIC)
                                    .ToList();

            foreach (var history in magicCardsHistory)
            {
                var card = history.UsedCard;
                var requestTriggerCard = new RequestTryTriggerCardBattleAction(card, true);
                context.ActionScheduler.Enqueue(requestTriggerCard);
            }

            observer?.Clean();
            observer = null;
        }
    }
}