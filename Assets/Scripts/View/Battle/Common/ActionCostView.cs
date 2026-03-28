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
        private int currentViewMaxCost;

        private int targetViewCost;
        private int targetViewMaxCost;

        public override void OnInitialized()
        {
            eventBus.Subscribe<InitialActionCostSettled>(OnInitialActionCostSettled);
            eventBus.Subscribe<CostConsumed>(OnCostConsumed);
            eventBus.Subscribe<CostRestored>(OnCostRestored);
            eventBus.Subscribe<MaxCostChanged>(OnMaxCostChanged); 
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialActionCostSettled>(OnInitialActionCostSettled);
            eventBus?.Unsubscribe<CostConsumed>(OnCostConsumed);
            eventBus?.Unsubscribe<CostRestored>(OnCostRestored);
            eventBus?.Unsubscribe<MaxCostChanged>(OnMaxCostChanged);
        }

        private void OnInitialActionCostSettled(InitialActionCostSettled payload)
        {
            actionCost = payload.ActionCost;
            
            targetViewCost = actionCost.RemainCost;
            targetViewMaxCost = actionCost.MaxActionCost;
            
            SetCostText(targetViewCost, targetViewMaxCost);
        }

        private void OnCostConsumed(CostConsumed payload)
        {
            if (targetViewCost == payload.CurrentCost) return;
            
            targetViewCost = payload.CurrentCost;

            int snapshotCost = targetViewCost;
            int snapshotMax = targetViewMaxCost;

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CostConsumed_CountCost, 
                CountCostPresentation(snapshotCost, snapshotMax), 
                () => SetCostText(snapshotCost, snapshotMax));
        }

        private void OnCostRestored(CostRestored payload)
        {
            if (targetViewCost == payload.CurrentCost) return;

            targetViewCost = payload.CurrentCost;

            int snapshotCost = targetViewCost;
            int snapshotMax = targetViewMaxCost;

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CostRestored_CountCost, 
                CountCostPresentation(snapshotCost, snapshotMax), 
                () => SetCostText(snapshotCost, snapshotMax));
        }

        private void OnMaxCostChanged(MaxCostChanged payload)
        {
            if (targetViewCost == payload.CurrentAmount && targetViewMaxCost == payload.CurrentMax) return;

            targetViewCost = payload.CurrentAmount;
            targetViewMaxCost = payload.CurrentMax;

            int snapshotCost = targetViewCost;
            int snapshotMax = targetViewMaxCost;

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CostRestored_CountCost, 
                CountCostPresentation(snapshotCost, snapshotMax), 
                () => SetCostText(snapshotCost, snapshotMax));
        }
        
        private IEnumerator CountCostPresentation(int finalCost, int finalMaxCost)
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.Join(DOTween.To(() => currentViewCost, x => currentViewCost = x, finalCost, costCountDuration));
            sequence.Join(DOTween.To(() => currentViewMaxCost, x => currentViewMaxCost = x, finalMaxCost, costCountDuration));
            
            sequence.SetEase(costCountEasingType);
            
            sequence.OnUpdate(() => 
            {
                costIndicator.text = $"{currentViewCost}/{currentViewMaxCost}";
            });

            yield return sequence.WaitForCompletion();
        }

        private void SetCostText(int currentCost, int maxCost)
        {
            currentViewCost = currentCost;
            currentViewMaxCost = maxCost;
            costIndicator.text = $"{currentViewCost}/{currentViewMaxCost}";
        }
    }
}