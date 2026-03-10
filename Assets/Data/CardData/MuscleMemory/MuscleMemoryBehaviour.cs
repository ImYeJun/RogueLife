using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class MuscleMemory : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        private class Observer
        {
            private BattleContext context;
            private Guid requestId;

            public Observer(BattleContext context, Guid requestId)
            {
                this.context = context;
                this.requestId = requestId;
            }

            public void PostDrawCard(DrawCardBattleAction drawCardAction, BattleContext context)
            {
                if (drawCardAction.RequestID != requestId) { return; }

                var applyReflectionAction = new ApplyReflectEffectOnCard(drawCardAction.Card);
                context.ActionScheduler.Enqueue(applyReflectionAction);

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

            public void CleanItself()
            {
                context.ActionObserverHub.UnsubscribePostObserver<DrawCardBattleAction>(PostDrawCard);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
                context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(OnPlayerTurnEnd);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MuscleMemory() {}
        private MuscleMemory(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType)
        : base(owner, targetType, reflectionTargetType) { }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new MuscleMemory(owner, targetType, reflectionTargetType);
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
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, Guid.NewGuid());
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            Guid requestId = Guid.NewGuid();
            
            ExecuteCommonAction(context, requestId);

            var observer = new Observer(context, requestId);
            context.ActionObserverHub.SubscribePostObserver<DrawCardBattleAction>(observer.PostDrawCard);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
            context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(observer.OnPlayerTurnEnd);
        }
        private void ExecuteCommonAction(BattleContext context, Guid requestId)
        {
            var requestDrawAction = new RequestDrawCardBattleAction(requestId);
            context.ActionScheduler.Enqueue(requestDrawAction);
        }
    }
}