using System;
using System.Collections.Generic;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.TransactionSelectView
{
    public class TransactionSelectView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
    {
        [Serializable]
        private struct ChoiceButtonMapping
        {
            [SerializeField] private TransactionChoiceOrder targetOrder;
            [SerializeField] private TransactionChoiceButton choiceButton;

            public TransactionChoiceOrder TargetOrder => targetOrder;
            public TransactionChoiceButton ChoiceButton => choiceButton;
        }

        [SerializeField] private GameObject uiRoot;
        [SerializeField] private List<ChoiceButtonMapping> buttonMappings;

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
        }
        
        private void OnChoiceSelected(TransactionChoiceOrder selectedOrder)
        {
            commander.SettleTransactionChoice(selectedOrder);
            
            foreach (var mapping in buttonMappings)
            {
                mapping.ChoiceButton.Unactive();
            }

            uiRoot.SetActive(false); 
        }
    }
}