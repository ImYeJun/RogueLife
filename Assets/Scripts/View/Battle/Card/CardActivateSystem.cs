using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using System.Collections.Generic;
using System.Collections;

namespace View.BattleView
{
    public class CardActivateSystem : InteractableViewBehaviour<IBattleViewEvent, IBattleViewCommander>
    {
        [SerializeField] private PlayerTurnEndButton playerTurnEndButton;
        [SerializeField] private CardTargetSelectSystem targetSelectSystem;
        private IReadOnlyBattleActionCost actionCost;

        private struct TargetRequest
        {
            public Card Card;
            public bool IsTriggering;
            public Action<Card, CardTarget> OnTargetSelected;
            public int SequenceId;
            public int PresentationPriority;

            public TargetRequest(Card card, bool isTriggering, Action<Card, CardTarget> onTargetSelected, int sequenceId, int presentationPriority)
            {
                Card = card;
                OnTargetSelected = onTargetSelected;
                IsTriggering = isTriggering;
                SequenceId = sequenceId;
                PresentationPriority = presentationPriority;
            }
        }

        private Queue<TargetRequest> targetingQueue = new Queue<TargetRequest>();
        private bool isTargeting = false;

        public Func<Card, bool, IEnumerator> OnCardProcessingPrepared { get; set; }
        public Action<bool> SetHandCardInteractable { get; set; }
        public Func<bool> IsProcessingCard { get; set; }

        public override void OnInitialized()
        {
            eventBus.Subscribe<InitialActionCostSettled>(OnInitialActionCostSettled);
            eventBus.Subscribe<UseCardRequested>(OnUseCardRequested);
            eventBus.Subscribe<TriggerCardRequested>(OnTriggerCardRequested);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialActionCostSettled>(OnInitialActionCostSettled);
            eventBus?.Unsubscribe<UseCardRequested>(OnUseCardRequested);
            eventBus?.Unsubscribe<TriggerCardRequested>(OnTriggerCardRequested);
        }

        public void OnInitialActionCostSettled(InitialActionCostSettled payload)
        {
            actionCost = payload.ActionCost;
        }

        public void UseCard(Card card)
        {
            if (actionCost == null)
            {
                throw new InvalidOperationException("[CardActivateSystem] ActionCost is not initialized yet.");
            }

            if (!actionCost.HasEnough(card.CurrentActionCost))
            {
                Debug.Log($"[CardActivateSystem] Not enough action cost. Required: {card.CurrentActionCost}");
                return;
            }

            EnqueueTargetRequest(card, false, (c, t) => ActivateCard(c, t, false, false), 0, 0);
        }

        private void OnUseCardRequested(UseCardRequested payload)
        {
            EnqueueTargetRequest(payload.Card, false, (card, target) => ActivateCard(card, target, payload.IsFreeUse, false), payload.SequenceId, 0);
        }
        
        private void OnTriggerCardRequested(TriggerCardRequested payload)
        {
            EnqueueTargetRequest(payload.Card, true, (card, target) => TriggerCard(card, target, payload.IsReflection, true), payload.SequenceId, 0);
        }

        private void EnqueueTargetRequest(Card card, bool isTriggering, Action<Card, CardTarget> onTargetSelected, int sequenceId, int presentationPriority)
        {
            targetingQueue.Enqueue(new TargetRequest(card, isTriggering, onTargetSelected, sequenceId, presentationPriority));
            
            if (!isTargeting)
            {
                playerTurnEndButton.SetTargetingState(true); 
                ProcessNextRequest();
            }
        }

        private void ProcessNextRequest()
        {
            if (targetingQueue.Count == 0)
            {
                isTargeting = false;
                playerTurnEndButton.SetTargetingState(false); 
                return;
            }

            isTargeting = true;
            TargetRequest request = targetingQueue.Dequeue();

            StartCoroutine(ProcessTargetingRoutine(request));
        }

        private IEnumerator ProcessTargetingRoutine(TargetRequest request)
        {
            yield return new WaitWhile(() => IsProcessingCard.Invoke());

            SetHandCardInteractable.Invoke(false);
            bool isProcessPresentationEnd = false;
            presentationManager.Enqueue(request.SequenceId, request.PresentationPriority, OnCardProcessingPrepared.Invoke(request.Card, request.IsTriggering),
                () =>
                {
                    isProcessPresentationEnd = true;
                }
            );

            yield return new WaitUntil(() => isProcessPresentationEnd);

            bool isTargetSelected = false;
            targetSelectSystem.RequestTarget(request.Card, (c, t) => 
            {
                request.OnTargetSelected(c, t); 
                isTargetSelected = true;
            });

            yield return new WaitUntil(() => isTargetSelected);

            ProcessNextRequest();           
        }

        private void ActivateCard(Card card, CardTarget cardTarget, bool isFreeUse, bool isTriggering)
        {
            if (!commander.IsAbleToUseCard(card, cardTarget))
            {
                Debug.Log($"[CardActivateSystem] Cannot activate card: {card.CurrentName}");
                commander.CancelActivation(card, isTriggering);
                return;
            }

            commander.UseCard(card, cardTarget, isFreeUse);
        }

        private void TriggerCard(Card card, CardTarget cardTarget, bool isReflection, bool isTriggering)
        {
            if (!commander.IsAbleToUseCard(card, cardTarget))
            {
                Debug.Log($"[CardActivateSystem] Cannot trigger card: {card.CurrentName}");
                commander.CancelActivation(card, isTriggering);
                return;
            }

            commander.TriggerCard(card, cardTarget, isReflection);
        }
    }
}