using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace View.ScheduleView.Deck
{
    public class CostFilterListButton : MonoBehaviour
    {
        [SerializeField] protected int cost;
        [SerializeField] protected TextMeshProUGUI costText;
        
        protected Action<int> onPressed;

        public virtual void OnPressed()
        {
            onPressed.Invoke(cost);
        }

        public virtual void SetState(HashSet<int> filteringCost)
        {
            string symbol = filteringCost.Contains(cost) ? "●" : "○";
            
            costText.text = symbol + $" {cost}";
        }

        public void SetOnPressed(Action<int> toggleCostFilteringState)
        {
            onPressed = toggleCostFilteringState;
        }
    }
}