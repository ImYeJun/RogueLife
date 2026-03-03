using System;
using System.Collections.Generic;
using UnityEngine;

namespace View.ScheduleView.Deck
{
    public class FilteringSettingView : MonoBehaviour {
        [SerializeField] private AttributeFilterButton attributeFilterButton;
        [SerializeField] private TypeFilterButton typeFilterButton;
        [SerializeField] private CostFilteringButton costFilteringButton;

        public void SetState((HashSet<CardAttribute> attributes, HashSet<CardType> filteringType, HashSet<int> filteringCost) filters)
        {
            attributeFilterButton.SetState(filters.attributes);
            typeFilterButton.SetState(filters.filteringType);
            costFilteringButton.SetState(filters.filteringCost);
        }

        public void SetOnButtonPressed(Action<CardAttribute> toggleAttributeFilteringState, Action<CardType> toggleTypeFilteringState, Action<int> toggleCostFilteringState)
        {
            attributeFilterButton.SetOnListButtonPressed(toggleAttributeFilteringState);
            typeFilterButton.SetOnListButtonPressed(toggleTypeFilteringState);
            costFilteringButton.SetOnListButtonPressed(toggleCostFilteringState);
        }

        public void Initialize()
        {
            attributeFilterButton.Initialize();
            typeFilterButton.Initialize();
            costFilteringButton.Initialize();
        }
    }
}