using System;
using System.Collections.Generic;

public class SequentialCardUseRequestBattleAction : IBattleAction
{
    private List<Card> cards;
    private bool isFreeUse;

    private class SequenceObserver
    {
        private BattleContext context;
        private List<Card> cards;
        private int currentIndex;
        private bool isFreeUse;

        public SequenceObserver(BattleContext context, List<Card> cards, bool isFreeUse)
        {
            this.context = context;
            this.cards = cards;
            this.currentIndex = 0;
            this.isFreeUse = isFreeUse;
        }

        public void OnCardExecutionCompleted(CardExecutionCompletedBattleEvent payload)
        {
            if (currentIndex >= cards.Count) return;

            if (payload.Card == cards[currentIndex])
            {
                currentIndex++;

                if (currentIndex < cards.Count)
                {
                    context.ActionScheduler.Enqueue(new RequestTryUseCardBattleAction(cards[currentIndex], isFreeUse));
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

    public SequentialCardUseRequestBattleAction(List<Card> cards, bool isFreeUse = true)
    {
        this.cards = cards;
        this.isFreeUse = isFreeUse;
    }

    public void Execute(BattleContext context)
    {
        if (cards == null || cards.Count == 0) return;

        var observer = new SequenceObserver(context, cards, isFreeUse);
        context.EventBus.Subscribe<CardExecutionCompletedBattleEvent>(observer.OnCardExecutionCompleted);
        context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);

        context.ActionScheduler.Enqueue(new RequestTryUseCardBattleAction(cards[0], isFreeUse));
    }
}