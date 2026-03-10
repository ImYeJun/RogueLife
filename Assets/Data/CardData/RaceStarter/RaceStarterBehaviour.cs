using System;
using System.ComponentModel;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class RaceStarter : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        private class Observer
        {
            private int observeCount;
            private BattleContext context;

            public Observer(int observeCount, BattleContext context)
            {
                this.observeCount = observeCount;
                this.context = context;
            }

            public void IncreaseExecuteTimes(UseCardBattleAction useCardAcion, BattleContext context)
            {
                useCardAcion.ExecuteTimes *= 2;

                if (--observeCount <= 0) { CleanItself(); }
            }

            public void OnBattleEnd(BattleEndBattleEvent payload)
            {
                CleanItself();
            }

            private void CleanItself()
            {
                context.ActionObserverHub.UnsubscribeActionModifier<UseCardBattleAction>(IncreaseExecuteTimes);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RaceStarter() {}
        private RaceStarter(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new RaceStarter(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, NoneCardTarget target)
        {
            return !context.BattleDeckHistory.HasPlayedCard(BattleScope.PHASE);
        }
        public override bool OnIsAbleToUseReflect(BattleContext context, NoneCardTarget target)
        {
            return !context.BattleDeckHistory.HasPlayedCard(BattleScope.PHASE);
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, 1);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, 2);
        }
        private static void ExecuteCommonAction(BattleContext context, int observeCount)
        {
            var observer = new Observer(observeCount, context);
            context.ActionObserverHub.SubscribeActionModifier<UseCardBattleAction>(observer.IncreaseExecuteTimes);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
        }
    }
}