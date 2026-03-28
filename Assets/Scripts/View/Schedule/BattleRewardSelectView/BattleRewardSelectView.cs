using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class BattleRewardSelectView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
    {
        [Header("Behaviour")]
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private CanvasGroup rootCanvasGroup;
        [SerializeField] private GameObject rewardButtonPrefab;
        [SerializeField] private Transform buttonsContainer;
        [SerializeField] private GameObject rewardCancleButton;
        private List<BattleRewardButton> activeRewardIcons = new List<BattleRewardButton>();
        private bool isSelecting;
        private Action requestNextNodeSelect;

        [Header("Presentation")]
        [SerializeField] private float fadeInDuration;
        [SerializeField] private Ease fadeInEase;
        [SerializeField] private float fadeOutDuration;
        [SerializeField] private Ease fadeOutEase;

        public override void OnInitialized()
        {
            uiRoot.SetActive(false);
            eventBus.Subscribe<BattleRewardSelectRequested>(OnBattleRewardSelectRequested);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<BattleRewardSelectRequested>(OnBattleRewardSelectRequested);
        }

        private void OnBattleRewardSelectRequested(BattleRewardSelectRequested payload)
        {
            foreach (var icon in activeRewardIcons)
            {
                Destroy(icon.gameObject);
            }
            activeRewardIcons.Clear();

            var candidates = payload.RewardCollector.RewardCandidates;
            foreach (var candidate in candidates)
            {
                var rewardGameObject = Instantiate(rewardButtonPrefab, buttonsContainer);

                var button = rewardGameObject.GetComponent<BattleRewardButton>();
                button.SetVisible(false);
                button.Initiate(candidate, OnButtonSelected, commander);

                button.transform.SetAsLastSibling();
                activeRewardIcons.Add(button);
            }
            
            var closeButtonBehaviour = rewardCancleButton.GetComponent<NoneBattleRewardButton>();
            closeButtonBehaviour.SetVisible(false);
            closeButtonBehaviour.Initiate(OnButtonSelected);
            rewardCancleButton.transform.SetAsLastSibling();
            LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsContainer.GetComponent<RectTransform>());

            requestNextNodeSelect = payload.RequestNextNodeSelect;

            isSelecting = true;
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.BattleRewardSelectRequested_Open, OpenPresentation());
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.BattleRewardSelectRequested_Close, ClosePresentation());
        }

        private IEnumerator OpenPresentation()
        {
            uiRoot.SetActive(true);
            yield return rootCanvasGroup.DOFade(1, fadeInDuration).From(0).SetEase(fadeInEase).WaitForCompletion();

            var sequence = DOTween.Sequence();
            foreach (var buttonBehaviour in activeRewardIcons)
            {
                sequence.Append(buttonBehaviour.PlayShowPresentation());
            }
            sequence.Append(rewardCancleButton.GetComponent<NoneBattleRewardButton>().PlayShowPresentation());
            sequence.Play();

            yield return new WaitWhile(() => isSelecting);
        }

        private IEnumerator ClosePresentation()
        {
            yield return rootCanvasGroup.DOFade(0, fadeOutDuration).From(1).SetEase(fadeOutEase).WaitForCompletion();
            uiRoot.SetActive(false);
            requestNextNodeSelect?.Invoke();
            requestNextNodeSelect = null;
        }

        private void OnButtonSelected()
        {
            isSelecting = false;
        }
    }
}
