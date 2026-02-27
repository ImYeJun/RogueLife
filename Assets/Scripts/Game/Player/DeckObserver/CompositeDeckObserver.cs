using System;
using System.Collections.Generic;
using UnityEngine;

namespace Field.Deck.Observers
{
    [Serializable]
    public class Composite : IDeckObserver
    {
        [SerializeReference, SubclassSelector] List<IDeckObserver> observers = new List<IDeckObserver>();

        public void OnCardEquipped(Card card)
        {
            foreach (var observer in observers)
            {
                observer.OnCardEquipped(card);
            }
        }

        public void OnCardRemoved(Card card)
        {
            foreach (var observer in observers)
            {
                observer.OnCardRemoved(card);
            }
        }

        public void OnStartObserving(List<Card> owningCards)
        {
            foreach (var observer in observers)
            {
                observer.OnStartObserving(owningCards);
            }
        }

        public void OnStopObserving(List<Card> owningCards)
        {
            foreach (var observer in observers)
            {
                observer.OnStopObserving(owningCards);
            }
        }
    }
}