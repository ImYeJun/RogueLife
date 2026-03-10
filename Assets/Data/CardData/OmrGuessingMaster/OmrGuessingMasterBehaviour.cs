using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class OmrGuessingMaster : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        private class Observer
        {
            private BattleContext context;
            private Guid requestId;
            private PlayerCardTarget playerCardTarget;
            private int successHealAmount;

            public Observer(BattleContext context, Guid requestId, PlayerCardTarget playerCardTarget, int successHealAmount)
            {
                this.context = context;
                this.requestId = requestId;
                this.playerCardTarget = playerCardTarget;
                this.successHealAmount = successHealAmount;
            }

            public void PostDrawCard(DrawCardBattleAction drawCard, BattleContext context)
            {
                if (drawCard.RequestID != requestId) { return; }

                IBattleAction determinedAction;
                if (drawCard.Card.CurrentAttribute == CardAttribute.LUCK)
                {
                    determinedAction = new HealEntityBattleAction(playerCardTarget.Player, successHealAmount);
                }
                else
                {
                    determinedAction = new MoveCardToDeckBattleAction(drawCard.Card, BattleDeckType.DRAW);
                }

                context.ActionScheduler.Enqueue(determinedAction);

                CleanItself();
            }

            public void OnBattleEnd(BattleEndBattleEvent payload)
            {
                CleanItself();
            }

            public void OnPlayerTurnEnd(PlayerTurnEndBattleEvent payload)
            {
                CleanItself();
            }

            private void CleanItself()
            {
                context.ActionObserverHub.UnsubscribePostObserver<DrawCardBattleAction>(PostDrawCard);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
                context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(OnPlayerTurnEnd);
            }
        }
        
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OmrGuessingMaster() {}
        private OmrGuessingMaster(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new OmrGuessingMaster(owner, targetType, reflectionTargetType);
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
            ExecuteCommonAction(context, target, 10);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, 20);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, int successHealAmount)
        {
            var requestId = Guid.NewGuid();
            var requestDrawAction = new RequestDrawCardBattleAction(requestId);
            context.ActionScheduler.Enqueue(requestDrawAction);

            var observer = new Observer(context, requestId, target, successHealAmount);
            context.ActionObserverHub.SubscribePostObserver<DrawCardBattleAction>(observer.PostDrawCard);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
            context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(observer.OnPlayerTurnEnd);
        }
    }
}