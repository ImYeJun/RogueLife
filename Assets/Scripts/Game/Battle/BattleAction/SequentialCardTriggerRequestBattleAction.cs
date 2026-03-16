using System;
using System.Collections.Generic;

public class SequentialCardTriggerRequestBattleAction : IBattleAction
{
    private List<Card> cards;
    private bool isReflection;

    private class TriggerSequenceObserver
    {
        private BattleContext context;
        private List<Card> cards;
        private int currentIndex;
        private bool isReflection;

        public TriggerSequenceObserver(BattleContext context, List<Card> cards, bool isReflection)
        {
            this.context = context;
            this.cards = cards;
            this.currentIndex = 0;
            this.isReflection = isReflection;
        }

        public void OnCardExecutionCompleted(CardExecutionCompletedBattleEvent payload)
        {
            if (currentIndex >= cards.Count) return;

            if (payload.Card == cards[currentIndex])
            {
                currentIndex++;

                if (currentIndex < cards.Count)
                {
                    context.ActionScheduler.Enqueue(new RequestTryTriggerCardBattleAction(cards[currentIndex], isReflection));
                }
                else
                {
                    CleanItself();
                }
            }
        }

        public void OnBattleEnd(BattleEndBattleEvent payload)
        {
            CleanItself();
        }

        public void CleanItself()
        {
            context.EventBus.Unsubscribe<CardExecutionCompletedBattleEvent>(OnCardExecutionCompleted);
            context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
        }
    }

    public SequentialCardTriggerRequestBattleAction(List<Card> cards, bool isReflection = false)
    {
        this.cards = cards;
        this.isReflection = isReflection;
    }

    public void Execute(BattleContext context)
    {
        if (cards == null || cards.Count == 0) return;

        var observer = new TriggerSequenceObserver(context, cards, isReflection);
        context.EventBus.Subscribe<CardExecutionCompletedBattleEvent>(observer.OnCardExecutionCompleted);
        context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);

        context.ActionScheduler.Enqueue(new RequestTryTriggerCardBattleAction(cards[0], isReflection));
    }
}