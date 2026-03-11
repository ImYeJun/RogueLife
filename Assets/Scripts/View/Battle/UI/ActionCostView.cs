using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using TMPro;

namespace View.BattleView
{
    public class ActionCostView : ViewBehaviour<IBattleViewEvent>
    {
        [SerializeField] private TextMeshProUGUI costIndicator;
        private IReadOnlyBattleActionCost actionCost;

        public override void OnInitialized()
        {
            eventBus.Subscribe<InitialActionCostSettled>(OnInitialActionCostSettled);
            eventBus.Subscribe<CostConsumed>(OnCostConsumed);
            eventBus.Subscribe<CostRestored>(OnCostRestored);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialActionCostSettled>(OnInitialActionCostSettled);
            eventBus?.Unsubscribe<CostConsumed>(OnCostConsumed);
            eventBus?.Unsubscribe<CostRestored>(OnCostRestored);
        }

        private void OnInitialActionCostSettled(InitialActionCostSettled payload)
        {
            actionCost = payload.ActionCost;

            SetCostText();
        }

        private void OnCostConsumed(CostConsumed payload)
        {
            SetCostText(payload.CurrentCost);
        }

        private void OnCostRestored(CostRestored payload)
        {
            SetCostText(payload.CurrentCost);
        }
        
        private void SetCostText()
        {
            SetCostText(actionCost.RemainCost);
        }
        private void SetCostText(int currentCost)
        {
            costIndicator.text = $"{currentCost}/{actionCost.MaxActionCost}";
        }
    }
}
