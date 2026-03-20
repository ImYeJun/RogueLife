using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;
using Field.Deck.Observers;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class MentalTraining : CardBattleBehaviour<PlayerCardTarget, NoneCardTarget>
    {
        private class MentalTrainingDeckObserver : IDeckObserver
        {
            private BattleContext context;
            private int remainObserveCount;
            
            private Dictionary<Card, CardCostModifier> discountedCards = new Dictionary<Card, CardCostModifier>();

            public MentalTrainingDeckObserver(BattleContext context, int remainObserveCount)
            {
                this.context = context;
                this.remainObserveCount = remainObserveCount;
            }

            public void OnStartObserving(List<Card> owningCards)
            {
                foreach (var card in owningCards)
                {
                    ApplyDiscount(card);
                }

                context.ActionObserverHub.SubscribePostObserver<TryUseCardBattleAction>(PostObserveTryUseCard);
            }

            public void OnCardEquipped(Card card)
            {
                ApplyDiscount(card);
            }

            public void OnCardRemoved(Card card)
            {
                RemoveDiscount(card);
            }

            public void OnStopObserving(List<Card> owningCards)
            {
                var cardsToRestore = discountedCards.Keys.ToList();
                foreach (var card in cardsToRestore)
                {
                    RemoveDiscount(card);
                }

                context.ActionObserverHub.UnsubscribePostObserver<TryUseCardBattleAction>(PostObserveTryUseCard);
            }

            private void ApplyDiscount(Card card)
            {
                if (!discountedCards.ContainsKey(card))
                {
                    var modifier = new CardCostModifier(-1);
                    
                    // 💡 [수정됨] 직접 추가(card.AddCostModifier)하는 대신 범용 Action을 스케줄러 큐에 넣습니다!
                    context.ActionScheduler.Enqueue(new AddCardCostModifierBattleAction(card, modifier));
                    
                    discountedCards.Add(card, modifier);
                }
            }

            private void RemoveDiscount(Card card)
            {
                if (discountedCards.TryGetValue(card, out var modifier))
                {
                    // 💡 [수정됨] 직접 제거(card.RemoveCostModifier)하는 대신 범용 Action을 스케줄러 큐에 넣습니다!
                    context.ActionScheduler.Enqueue(new RemoveCardCostModifierBattleAction(card, modifier));
                    
                    discountedCards.Remove(card);
                }
            }

            public void PostObserveTryUseCard(TryUseCardBattleAction action, BattleContext context)
            {
                // (선택적) 카드가 모종의 이유로 사용이 취소되었다면 카운트를 차감하지 않도록 방어 코드를 넣어도 좋습니다.
                // if (action.IsNullified) { return; }

                RemoveDiscount(action.Card);

                remainObserveCount--;

                if (remainObserveCount <= 0)
                {
                    context.DeckSystem.UnregisterHandDeckObserver(this);
                }
            }
        }

        [SerializeField] private BattleStatusEffectEntity lightBodyEntity;
        
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MentalTraining() {}
        private MentalTraining(ICardBehaviourOwner owner, BattleStatusEffectEntity lightBodyEntity, CardTargetType targetType, CardTargetType reflectionTargetType) : base(owner, targetType, reflectionTargetType) 
        { 
            this.lightBodyEntity = lightBodyEntity;
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new MentalTraining(owner, this.lightBodyEntity, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, PlayerCardTarget target)
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

        protected override void OnExecute(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            var lightBody = new BattleStatusEffect(lightBodyEntity, 1, 1);
            var applyBuffAction = new ApplyEntityStatusEffectBattleAction(target.Player, lightBody);
            context.ActionScheduler.Enqueue(applyBuffAction);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            var observer = new MentalTrainingDeckObserver(context, 3);
            context.DeckSystem.RegisterHandDeckObserver(observer);
        }
    }
}