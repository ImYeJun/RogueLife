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

        public override void OnInitialized()
        {
            eventBus.Subscribe<InitialActionCostSettled>(OnInitialActionCostSettled);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialActionCostSettled>(OnInitialActionCostSettled);
        }

        public void OnInitialActionCostSettled(InitialActionCostSettled payload)
        {
            actionCost = payload.ActionCost;
        }

        public void UseCard(Card card)
        {
            if (!actionCost.HasEnough(card.CurrentActionCost))
            {
                Debug.Log($"{card.CurrentActionCost} 만큼의 행동력이 없음");
                return;
            }

            targetSelectSystem.RequestTarget(card, ActivateCard);
        }

        public void ActivateCard(Card card, CardTarget cardTarget)
        {
            if (!commander.IsAbleToUseCard(card, cardTarget))
            {
                Debug.Log($"{card.CurrentActionCost} 실행 불가");
                return;
            }

            commander.UseCard(card, cardTarget);
            targetSelectSystem.ClearAllTargetables();
        }
    }
}
