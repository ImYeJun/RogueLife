using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace View.ScheduleView.Deck
{
    public class DeckInventorySorter
    {
        private SortingState sortingState;
        private HashSet<CardAttribute> filteringAttributes;
        private HashSet<CardType> filteringType;
        private HashSet<int> filteringCost;

        public SortingState SortingState { get => sortingState; }
        public HashSet<CardAttribute> FilteringAttributes { get => filteringAttributes; }
        public HashSet<CardType> FilteringType { get => filteringType; }
        public HashSet<int> FilteringCost { get => filteringCost; }

        public DeckInventorySorter()
        {
            Initialize();
        }

        public void Initialize()
        {
            sortingState = new SortingState(SortingType.ObtainDate, Order.Ascending);
            filteringAttributes = new HashSet<CardAttribute>();
            filteringType = new HashSet<CardType>();
            filteringCost = new HashSet<int>();
        }

        public void ChangeSortingState(SortingType type) {
            SortingState newState;

            if (type == sortingState.Type)
            {
                newState = new SortingState(type, sortingState.Order == Order.Ascending ? Order.Descending : Order.Ascending);
            }
            else
            {
                newState = new SortingState(type, Order.Ascending);
            }

            sortingState = newState;
        }
        
        public void ToggleAttributeFilter(CardAttribute attr) 
        {
            if (filteringAttributes.Contains(attr)) filteringAttributes.Remove(attr);
            else filteringAttributes.Add(attr);
        }
        
        public void ToggleTypeFilter(CardType type) 
        {
            if (filteringType.Contains(type)) filteringType.Remove(type);
            else filteringType.Add(type);
        }
        
        public void ToggleCostFilter(int cost) 
        {
            if (filteringCost.Contains(cost)) filteringCost.Remove(cost);
            else filteringCost.Add(cost);
        }

        public List<Card> ProcessDeck(IReadOnlyDictionary<CardData, List<Card>> deck)
        {
            var allCards = deck.Values.SelectMany(sel => sel);
            var filteredCards = FilterDeck(allCards);
            
            return SortDeck(filteredCards);
        }

        private IEnumerable<Card> FilterDeck(IEnumerable<Card> cards)
        {
            return cards.Where(card => 
                (filteringAttributes.Count == 0 || filteringAttributes.Contains(card.CurrentAttribute)) &&
                (filteringType.Count == 0 || filteringType.Contains(card.CurrentType)) &&
                (filteringCost.Count == 0 || filteringCost.Contains(card.CurrentActionCost >= 10 ? 10 : card.CurrentActionCost))
            );
        }

        private List<Card> SortDeck(IEnumerable<Card> deck)
        {
            switch (sortingState.Type)
            {
                case SortingType.ObtainDate:
                    return sortingState.Order == Order.Ascending ? 
                            deck.OrderBy(card => card.ObtainData).ToList() : 
                            deck.OrderByDescending(card => card.ObtainData).ToList();
                case SortingType.Name:
                    var culture = StringComparer.Create(CultureInfo.CreateSpecificCulture("ko-KR"), false);
                    return sortingState.Order == Order.Ascending ? 
                            deck.OrderBy(card => card.CurrentName, culture).ToList() : 
                            deck.OrderByDescending(card => card.CurrentName, culture).ToList();
                case SortingType.ActionCost:
                    return sortingState.Order == Order.Ascending ? 
                            deck.OrderBy(card => card.CurrentActionCost).ThenBy(card => card.ObtainData).ToList() : 
                            deck.OrderByDescending(card => card.CurrentActionCost).ThenBy(card => card.ObtainData).ToList();
                default:
                    throw new InvalidOperationException($"[DeckInventorySorter] {sortingState.Type} is not valid.");
            }
        }
    }
}