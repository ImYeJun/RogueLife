using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using System.Collections.Generic;

namespace View.BattleView
{
    public class CardActivateSystem : InteractableViewBehaviour<IBattleViewEvent, IBattleViewCommander>
    {
        [SerializeField] private CardTargetSelectSystem targetSelectSystem;
        private IReadOnlyBattleActionCost actionCost;

        private struct TargetRequest
        {
            public Card Card;
            public Action<Card, CardTarget> OnTargetSelected;

            public TargetRequest(Card card, Action<Card, CardTarget> onTargetSelected)
            {
                Card = card;
                OnTargetSelected = onTargetSelected;
            }
        }

        private Queue<TargetRequest> targetingQueue = new Queue<TargetRequest>();
        private bool isTargeting = false;

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
                throw new InvalidOperationException("[CardActivateSystem/UseCard] ActionCost is not initialized yet.");
            }

            if (!actionCost.HasEnough(card.CurrentActionCost))
            {
                Debug.Log($"[CardActivateSystem/UseCard] Not enough action cost. Required: {card.CurrentActionCost}");
                return;
            }

            EnqueueTargetRequest(card, (c, t) => ActivateCard(c, t, false));
        }

        private void OnUseCardRequested(UseCardRequested payload)
        {
            EnqueueTargetRequest(payload.Card, (card, target) => ActivateCard(card, target, payload.IsFreeUse));
        }
        private void OnTriggerCardRequested(TriggerCardRequested payload)
        {
            EnqueueTargetRequest(payload.Card, (card, target) => TriggerCard(card, target, payload.IsReflection));
        }

        private void EnqueueTargetRequest(Card card, Action<Card, CardTarget> onTargetSelected)
        {
            targetingQueue.Enqueue(new TargetRequest(card, onTargetSelected));
            
            if (!isTargeting)
            {
                ProcessNextRequest();
            }
        }

        private void ProcessNextRequest()
        {
            if (targetingQueue.Count == 0)
            {
                isTargeting = false;
                return;
            }

            isTargeting = true;
            TargetRequest request = targetingQueue.Dequeue();

            targetSelectSystem.RequestTarget(request.Card, (c, t) => 
            {
                request.OnTargetSelected(c, t); 
                ProcessNextRequest();           
            });
        }

        private void ActivateCard(Card card, CardTarget cardTarget, bool isFreeUse)
        {
            if (!commander.IsAbleToUseCard(card, cardTarget))
            {
                Debug.Log($"[CardActivateSystem/ActivateCard] Cannot activate card: {card.CurrentName}");
                return;
            }

            commander.UseCard(card, cardTarget, isFreeUse);
        }

        private void TriggerCard(Card card, CardTarget cardTarget, bool isReflection)
        {
            commander.TriggerCard(card, cardTarget, isReflection);
        }
    }
}