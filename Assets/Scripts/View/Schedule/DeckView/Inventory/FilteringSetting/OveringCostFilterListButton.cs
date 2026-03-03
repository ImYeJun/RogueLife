using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace View.ScheduleView.Deck
{
    public class OveringFilterListButton : CostFilterListButton
    {
        public override void OnPressed()
        {
            onPressed.Invoke(10);
        }

        public override void SetState(HashSet<int> filteringCost)
        {
            string symbol = filteringCost.Contains(cost) ? "●" : "○";
            
            costText.text = symbol + $" {cost}" + " + ";
        }
    }
}