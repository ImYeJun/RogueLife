#nullable enable

using System;
using System.ComponentModel;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class GiftFromFuture : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        private class Observer
        {
            private BattleContext context;
            private Guid requestId;
            private bool isReflection;

            public Observer(BattleContext context, Guid requestId, bool isReflection)
            {
                this.context = context;
                this.requestId = requestId;
                this.isReflection = isReflection;
            }

            public void PostDrawCard(DrawCardBattleAction drawCard, BattleContext context)
            {
                if (drawCard.RequestID != requestId) { return; }

                if (isReflection)
                {
                    // 💡 [수정됨] 범용 모디파이어 시스템으로 전환!
                    var mod = new CardCostModifier(-1);
                    var decreaseCardCostAction = new AddCardCostModifierBattleAction(drawCard.Card, mod);
                    context.ActionScheduler.EnqueueFront(decreaseCardCostAction);
                }

                var applyReflectAction = new ApplyReflectEffectOnCard(drawCard.Card);
                context.ActionScheduler.EnqueueFront(applyReflectAction);

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
                context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(OnPlayerTurnEnd);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public GiftFromFuture() {}
        private GiftFromFuture(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new GiftFromFuture(owner, targetType, reflectionTargetType);
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
            ExecuteCommonAction(context, false);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, true);
        }
        private void ExecuteCommonAction(BattleContext context, bool isReflection)
        {
            Guid requestId = Guid.NewGuid();

            var observer = new Observer(context, requestId, isReflection);
            context.ActionObserverHub.SubscribePostObserver<DrawCardBattleAction>(observer.PostDrawCard);
            context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(observer.OnPlayerTurnEnd);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);

            var requestDrawAction = new RequestDrawCardBattleAction(requestId);
            context.ActionScheduler.Enqueue(requestDrawAction);
        }
    }
}