using System;
using System.Collections.Generic;

namespace Field.Deck.Observers
{
    public interface IDeckObserver
    {
        public void OnStartObserving(List<Card> owningCards);
        public void OnCardEquipped(Card card);
        public void OnCardRemoved(Card card);
        public void OnStopObserving(List<Card> owningCards);
    }
}