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
        private IncidentNode currentNode;
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
            buttonsContainer.gameObject.SetActive(false);
            incidentImage.gameObject.SetActive(true);


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
            eventBus.Subscribe<NodeEntered>(OnNodeEntered);
            eventBus.Subscribe<NodeExited>(OnNodeExited);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<IncidentSelectRequested>(OnIncidentSelectRequested);
            eventBus?.Unsubscribe<NodeEntered>(OnNodeEntered);
            eventBus?.Unsubscribe<NodeExited>(OnNodeExited);
        }

        public void OnNodeEntered(NodeEntered payload)
        {
            if (payload.EnteringNode is not IncidentNode incidentNode) { return ;}

            currentNode = incidentNode;
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.NodeEnter_StageSet, SetStage(), () =>
            {
                uiRoot.SetActive(true);
            });
        }
        public IEnumerator SetStage()
        {
            yield return null;
        }

        public void OnNodeExited(NodeExited payload)
        {
            if (payload.ExitingNode != currentNode) { return; }
            currentNode = null;

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.NodeExit_StageUnset, UnsetStage(), () =>
            {
                if (payload.ExitingNode is IncidentNode)
                {
                    uiRoot.SetActive(false);
                    buttonsContainer.gameObject.SetActive(false);
                }
            });
        }
        public IEnumerator UnsetStage()
        {
            yield return null;
        }

        public void OnIncidentSelectRequested(IncidentSelectRequested payload)
        {
            if (payload.Data.Image is not null)
            {
                incidentImage.sprite = payload.Data.Image;
            }

            foreach (var choice in payload.Choices)
            {
                var button = pool.Get();
                button.transform.SetAsLastSibling();
                button.Initiate(choice, () => OnChoiceSelected(choice));
                activeButtons.Add(button);
            }

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.IncidentSelectRequested_ChoiceAppear, ChoiceButtonAppearPresentation());
        }

        private IEnumerator ChoiceButtonAppearPresentation()
        {
            buttonsContainer.gameObject.SetActive(true);
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsContainer as RectTransform);

            buttonsAppearTween?.Kill();
            buttonsAppearTween = DOTween.Sequence();
            
            float currentDelay = 0;
            foreach (var button in activeButtons)
            {
                button.SetVisible(false);
                buttonsAppearTween.Insert(currentDelay, button.PlayShowPresentation());
                currentDelay += buttonAppearDelay;
            }

            if (buttonsAppearTween != null && buttonsAppearTween.IsActive())
            {
                yield return buttonsAppearTween.WaitForCompletion();
            }
        }

        private void OnChoiceSelected(DeterminedIncidentChoice selectedChoice)
        {
            buttonsAppearTween?.Kill();
            buttonsContainer.gameObject.SetActive(false);
            commander.SettleIncidentChoice(selectedChoice);

            foreach (var button in activeButtons)
            {
                pool.Release(button);
            }
            activeButtons.Clear();
        }

#if UNITY_EDITOR
        [ContextMenu("Play Choice Button Appear Presentation")]
        public void TestPlayChoiceButtonAppearPresentation()
        {
            buttonsAppearTween?.Kill();
            buttonsAppearTween = DOTween.Sequence();

            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsContainer as RectTransform);
            float currentDelay = 0;
            foreach (var button in activeButtons)
            {
                buttonsAppearTween.Insert(currentDelay, button.PlayShowPresentation());
                currentDelay += buttonAppearDelay;
            }

            buttonsAppearTween.Pause();
            StartCoroutine(PlayDelay(ChoiceButtonAppearPresentation()));
        }

        private IEnumerator PlayDelay(IEnumerator presentation)
        {
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(presentation);
        }
#endif
    }
}