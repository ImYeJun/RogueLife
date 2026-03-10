using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class DejaVu : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {   
        private class Observer
        {
            private BattleContext context;

            public Observer(BattleContext context)
            {
                this.context = context;
            }

            public void PostTryUseCard(TryUseCardBattleAction tryUseCardAction, BattleContext context)
            {
                if (!tryUseCardAction.IsSuccess) { return; }

                var card = tryUseCardAction.Card;

                if (!card.IsReflectionApplied)
                {
                    var applyReflectionAction = new ApplyReflectEffectOnCard(card);
                    context.ActionScheduler.EnqueueFront(applyReflectionAction);
                }
            }

            public void OnPlayerTurnEnd(PlayerTurnEndBattleEvent payload)
            {
                CleanObserver();
            }
            public void OnBattleEnd(BattleEndBattleEvent payload)
            {
                CleanObserver();
            }

            private void CleanObserver() 
            {
                context.ActionObserverHub.UnsubscribePostObserver<TryUseCardBattleAction>(PostTryUseCard);
                context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(OnPlayerTurnEnd);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DejaVu() {}
        private DejaVu(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType)
        : base(owner, targetType, reflectionTargetType) { }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new DejaVu(owner, targetType, reflectionTargetType);
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
            if (context.Random.NextDouble() > 0.01) { return; }

            var cards = context.HandDeck.GetCards();

            for (int i = cards.Count - 1; i >= 0; i--)
            {
                var card = cards[i];
                var applyReflectionAction = new ApplyReflectEffectOnCard(card);
                context.ActionScheduler.Enqueue(applyReflectionAction);
            }
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            if (context.Random.NextDouble() > 0.01) { return; }

            var observer = new Observer(context);

            context.ActionObserverHub.SubscribePostObserver<TryUseCardBattleAction>(observer.PostTryUseCard);
            context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(observer.OnPlayerTurnEnd);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
        }
    }
}