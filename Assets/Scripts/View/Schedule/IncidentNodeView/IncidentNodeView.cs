using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.IncidentNodeView
{
    public class IncidentNodeView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
    {
        [Header("Behaviour")]
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private Transform buttonsContainer; 
        [SerializeField] private GameObject incidentButtonPrefab; 
        [SerializeField] private Image incidentImage; 

        [Header("Presentation")]
        [SerializeField] private float buttonAppearDelay;
        private Sequence buttonsAppearTween;

        private IObjectPool<IncidentChoiceButton> pool;
        private List<IncidentChoiceButton> activeButtons = new List<IncidentChoiceButton>();

        public override void OnInitialized()
        {
            uiRoot.SetActive(false);

            pool = new ObjectPool<IncidentChoiceButton>(
                createFunc: () =>
                {
                    var buttonObj = Instantiate(incidentButtonPrefab, buttonsContainer);
                    buttonObj.SetActive(false);
                    return buttonObj.GetComponent<IncidentChoiceButton>();
                },
                actionOnGet: (button) => { button.gameObject.SetActive(true); },
                actionOnRelease: (button) => { button.Unactive(); },
                actionOnDestroy: (button) => { Destroy(button.gameObject); },
                defaultCapacity: 4,
                maxSize: 10
            );

            activeButtons.Clear();
            eventBus.Subscribe<IncidentSelectRequested>(OnIncidentSelectRequested);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<IncidentSelectRequested>(OnIncidentSelectRequested);
        }

        public void OnIncidentSelectRequested(IncidentSelectRequested payload)
        {
            uiRoot.SetActive(true);

            if (payload.Data.Image is not null)
            {
                incidentImage.sprite = payload.Data.Image;
                incidentImage.gameObject.SetActive(false);
            }

            buttonsAppearTween?.Kill();
            buttonsAppearTween = DOTween.Sequence();
            
            foreach (var choice in payload.Choices)
            {
                var button = pool.Get();
                button.Initiate(choice, () => OnChoiceSelected(choice));
                activeButtons.Add(button);
            }

            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsContainer as RectTransform);
            float currentDelay = 0;
            foreach (var button in activeButtons)
            {
                buttonsAppearTween.Insert(currentDelay, button.PlayAppearPresentation());
                currentDelay += buttonAppearDelay;
            }

            buttonsAppearTween.Pause();
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.IncidentSelectRequested_ChoiceAppear, ChoiceButtonAppearPresentation());
        }

        private IEnumerator ChoiceButtonAppearPresentation()
        {
            buttonsAppearTween.Play();
            incidentImage.gameObject.SetActive(true);
            if (buttonsAppearTween != null && buttonsAppearTween.IsActive())
            {
                yield return buttonsAppearTween.WaitForCompletion();
            }
        }

        private void OnChoiceSelected(DeterminedIncidentChoice selectedChoice)
        {
            buttonsAppearTween?.Kill();
            commander.SettleIncidentChoice(selectedChoice);

            foreach (var button in activeButtons)
            {
                pool.Release(button);
            }
            activeButtons.Clear();

            uiRoot.SetActive(false);
        }
    }
}