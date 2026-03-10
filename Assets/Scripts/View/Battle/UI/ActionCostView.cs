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
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialActionCostSettled>(OnInitialActionCostSettled);
        }

        public void OnInitialActionCostSettled(InitialActionCostSettled payload)
        {
            actionCost = payload.ActionCost;

            SetCostText();
        }
        private void SetCostText()
        {
            costIndicator.text = $"{actionCost.RemainCost}/{actionCost.MaxActionCost}";
        }
    }
}
