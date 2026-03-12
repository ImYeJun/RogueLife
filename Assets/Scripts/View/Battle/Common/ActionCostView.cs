using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using TMPro;
using System.Collections;
using DG.Tweening;

namespace View.BattleView
{
    public class ActionCostView : ViewBehaviour<IBattleViewEvent>
    {
        [SerializeField] private TextMeshProUGUI costIndicator;
        [SerializeField] private float costCountDuration;
        [SerializeField] private Ease costCountEasingType;
        private IReadOnlyBattleActionCost actionCost;
        private int currentViewCost;

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
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CostConsumed_CountCost, CountCostPresentation(payload.CurrentCost), () => SetCostText(payload.CurrentCost));
        }

        private void OnCostRestored(CostRestored payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CostRestored_CountCost, CountCostPresentation(payload.CurrentCost), () => SetCostText(payload.CurrentCost));
        }
        
        private IEnumerator CountCostPresentation(int finalCost)
        {
            yield return DOTween.To(() => currentViewCost, (cost) => SetCostText(cost), finalCost, costCountDuration).SetEase(costCountEasingType).WaitForCompletion();
        }

        private void SetCostText()
        {
            SetCostText(actionCost.RemainCost);
        }
        private void SetCostText(int currentCost)
        {
            currentViewCost = currentCost;
            costIndicator.text = $"{currentViewCost}/{actionCost.MaxActionCost}";
        }
    }
}
