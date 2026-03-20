#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class ManaOverload : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        // 💡 [추가됨] 턴이 끝날 때 모디파이어(할인)를 회수하기 위한 일회용 옵저버
        private class TurnEndObserver
        {
            private BattleContext context;
            private Dictionary<Card, CardCostModifier> appliedModifiers;

            public TurnEndObserver(BattleContext context, Dictionary<Card, CardCostModifier> appliedModifiers)
            {
                this.context = context;
                this.appliedModifiers = appliedModifiers;
            }

            public void OnPlayerTurnEnd(PlayerTurnEndBattleEvent payload)
            {
                Clean();
            }

            public void OnBattleEnd(BattleEndBattleEvent payload)
            {
                Clean();
            }

            private void Clean()
            {
                foreach (var kvp in appliedModifiers)
                {
                    // 💡 턴이 종료되면 발급했던 영수증(Modifier)을 전부 강제 회수!
                    context.ActionScheduler.Enqueue(new RemoveCardCostModifierBattleAction(kvp.Key, kvp.Value));
                }
                
                context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(OnPlayerTurnEnd);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

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
            var modifiersDict = new Dictionary<Card, CardCostModifier>();

            for (int i = handCards.Count - 1; i >= 0; i--)
            {
                // 💡 [수정됨] 범용 모디파이어 시스템으로 전환!
                var mod = new CardCostModifier(-2);
                context.ActionScheduler.Enqueue(new AddCardCostModifierBattleAction(handCards[i], mod));
                modifiersDict[handCards[i]] = mod;
            }

            // 💡 [추가됨] 턴 종료 시 모디파이어를 회수하도록 이벤트를 구독합니다.
            if (modifiersDict.Count > 0)
            {
                var observer = new TurnEndObserver(context, modifiersDict);
                context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(observer.OnPlayerTurnEnd);
                context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
            }
        }
    }
}