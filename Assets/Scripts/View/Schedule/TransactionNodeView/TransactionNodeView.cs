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
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private List<ChoiceButtonMapping> buttonMappings;
        [SerializeField] private GameObject image;
        
        [Header("Presentation")]
        [SerializeField] private float buttonAppearDelay;
        private Sequence buttonsAppearTween;
        
        public override void OnInitialized()
        {
            uiRoot.SetActive(false);
            eventBus.Subscribe<TransactionSelectRequested>(OnTransactionSelectRequested);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<TransactionSelectRequested>(OnTransactionSelectRequested);
        }

        private void OnTransactionSelectRequested(TransactionSelectRequested payload)
        {
            uiRoot.SetActive(true);
            
            buttonsAppearTween?.Kill();
            buttonsAppearTween = DOTween.Sequence();
            image.gameObject.SetActive(false);


            float currentDelay = 0f;
            foreach (var mapping in buttonMappings)
            {
                if (payload.Choices.TryGetValue(mapping.TargetOrder, out var choiceData))
                {
                    mapping.ChoiceButton.Initiate(choiceData, () => OnChoiceSelected(mapping.TargetOrder));
                    
                    buttonsAppearTween.Insert(currentDelay, mapping.ChoiceButton.PlayAppearPresentation());
                    currentDelay += buttonAppearDelay;
                }
                else
                {
                    mapping.ChoiceButton.Unactive();
                }
            }
            
            buttonsAppearTween.Pause();
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.TransactionSelectRequested_ChoiceAppear, ChoiceButtonAppearPresentation());
        }

        private IEnumerator ChoiceButtonAppearPresentation()
        {
            image.gameObject.SetActive(true);
            buttonsAppearTween.Play();
            if (buttonsAppearTween != null && buttonsAppearTween.IsActive())
            {
                yield return buttonsAppearTween.WaitForCompletion();
            }
        }
        
        private void OnChoiceSelected(TransactionChoiceOrder selectedOrder)
        {
            buttonsAppearTween?.Kill();
            
            commander.SettleTransactionChoice(selectedOrder);
            
            foreach (var mapping in buttonMappings)
            {
                mapping.ChoiceButton.Unactive();
            }

            uiRoot.SetActive(false); 
        }
    }
}