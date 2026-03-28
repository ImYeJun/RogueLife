using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.TransactionNodeView
{
    public class TransactionNodeView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
    {
        [Serializable]
        private struct ChoiceButtonMapping
        {
            [SerializeField] private TransactionChoiceOrder targetOrder;
            [SerializeField] private TransactionChoiceButton choiceButton;

            public TransactionChoiceOrder TargetOrder => targetOrder;
            public TransactionChoiceButton ChoiceButton => choiceButton;
        }

        [Header("Behaviour")]
        private TransactionNode currentNode; 
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private Transform buttonsContainer; 
        [SerializeField] private List<ChoiceButtonMapping> buttonMappings;
        [SerializeField] private GameObject image;
        
        [Header("Presentation")]
        [SerializeField] private float buttonAppearDelay;
        private Sequence buttonsAppearTween;
        
        public override void OnInitialized()
        {
            uiRoot.SetActive(false);
            buttonsContainer.gameObject.SetActive(false); 
            
            eventBus.Subscribe<TransactionSelectRequested>(OnTransactionSelectRequested);
            eventBus.Subscribe<NodeEntered>(OnNodeEntered);
            eventBus.Subscribe<NodeExited>(OnNodeExited); 
        }
        
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<TransactionSelectRequested>(OnTransactionSelectRequested);
            eventBus?.Unsubscribe<NodeEntered>(OnNodeEntered);
            eventBus?.Unsubscribe<NodeExited>(OnNodeExited);
        }

        public void OnNodeEntered(NodeEntered payload)
        {
            if (payload.EnteringNode is not TransactionNode transactionNode) { return; }

            currentNode = transactionNode;
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.NodeEnter_StageSet, SetStage(), () =>
            {
                uiRoot.SetActive(true);
                image.gameObject.SetActive(true);
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
                if (payload.ExitingNode is TransactionNode)
                {
                    uiRoot.SetActive(false);
                    buttonsContainer.gameObject.SetActive(false);
                    image.gameObject.SetActive(false);
                }
            });
        }
        
        public IEnumerator UnsetStage()
        {
            yield return null;
        }

        private void OnTransactionSelectRequested(TransactionSelectRequested payload)
        {
            foreach (var mapping in buttonMappings)
            {
                if (payload.Choices.TryGetValue(mapping.TargetOrder, out var choiceData))
                {
                    mapping.ChoiceButton.Initiate(choiceData, () => OnChoiceSelected(mapping.TargetOrder));
                }
                else
                {
                    mapping.ChoiceButton.Unactive();
                }
            }
            
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.TransactionSelectRequested_ChoiceAppear, ChoiceButtonAppearPresentation());
        }

        private IEnumerator ChoiceButtonAppearPresentation()
        {
            buttonsContainer.gameObject.SetActive(true);
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsContainer as RectTransform);

            buttonsAppearTween?.Kill();
            buttonsAppearTween = DOTween.Sequence();

            float currentDelay = 0f;
            foreach (var mapping in buttonMappings)
            {
                if (mapping.ChoiceButton.gameObject.activeSelf) 
                {
                    mapping.ChoiceButton.SetVisible(false);
                    buttonsAppearTween.Insert(currentDelay, mapping.ChoiceButton.PlayShowPresentation());
                    currentDelay += buttonAppearDelay;
                }
            }

            if (buttonsAppearTween != null && buttonsAppearTween.IsActive())
            {
                yield return buttonsAppearTween.WaitForCompletion();
            }
        }
        
        private void OnChoiceSelected(TransactionChoiceOrder selectedOrder)
        {
            buttonsAppearTween?.Kill();
            buttonsContainer.gameObject.SetActive(false);
            
            commander.SettleTransactionChoice(selectedOrder);
            
            foreach (var mapping in buttonMappings)
            {
                mapping.ChoiceButton.Unactive();
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Play Choice Button Appear Presentation")]
        public void TestPlayChoiceButtonAppearPresentation()
        {
            foreach (var mapping in buttonMappings)
            {
                mapping.ChoiceButton.gameObject.SetActive(true);
            }
            
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