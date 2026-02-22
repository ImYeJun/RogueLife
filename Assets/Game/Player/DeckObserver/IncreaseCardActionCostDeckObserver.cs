using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Field.Deck.Observers
{
    [Serializable]
    public class IncreaseCardActionCost : IDeckObserver
    {
        private Dictionary<Card, CardCostModifier> costModifiers = new Dictionary<Card, CardCostModifier>();

        [SerializeField] private CardRarity minRarity;
        [SerializeField] private CardRarity maxRarity;
        [SerializeField] private CardType type;
        [SerializeField] private CardAttribute cardAttribute;
        [SerializeField] private bool isCheckBaseCost;
        [SerializeField] private List<int> baseCosts;
        [SerializeField] private int amount;

        public void OnCardEquipped(Card card)
        {
            if (costModifiers.ContainsKey(card))
            {
                UnityEngine.Debug.LogWarning("[IncreaseCardActionCost] The given card is already being observed");
                return;
            }

            AddModifierOn(card);
        }

        public void OnCardRemoved(Card card)
        {
            if (costModifiers.TryGetValue(card, out var modifier))
            {
                card.RemoveCostModifier(modifier);
                costModifiers.Remove(card);
                return;
            }

            UnityEngine.Debug.LogWarning("[IncreaseCardActionCost] The given card is not  being observed");
        }

        public void OnStartObserving(List<Card> owningCards)
        {
            //TODO Change argument type to HashSet<Card> more for safety
            
            foreach (var card in owningCards)
            {
                if (costModifiers.ContainsKey(card))
                {
                    UnityEngine.Debug.LogWarning("[IncreaseCardActionCost] The given card is already being observed");
                    continue;
                }

                AddModifierOn(card);
            }
        }

        public void OnStopObserving(List<Card> owningCards)
        {
            foreach (var keyValuePair in costModifiers)
            {
                var card = keyValuePair.Key;
                var modifier = keyValuePair.Value;

                card.RemoveCostModifier(modifier);
            }

            costModifiers.Clear();
        }
        
        private void AddModifierOn(Card card)
        {
            if (minRarity != CardRarity.ANY && card.CurrentRarity < minRarity) return;
            if (maxRarity != CardRarity.ANY && card.CurrentRarity > maxRarity) return;
            if (type != CardType.ANY && card.CurrentType != type) return;
            if (isCheckBaseCost && !baseCosts.Any(cost => cost == card.BaseActionCost)) return;
            if (cardAttribute != CardAttribute.ANY && card.CurrentAttribute != cardAttribute) return;

            var modifier = new CardCostModifier(amount);
            card.AddCostModifier(modifier);
            costModifiers[card] = modifier;
        }
    }
}