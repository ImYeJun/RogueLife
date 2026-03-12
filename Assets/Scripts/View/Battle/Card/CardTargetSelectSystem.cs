using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View.BattleView
{
    public class CardTargetSelectSystem : MonoBehaviour
    {
        // TODO: Refactor this code to reduce coupling
        [SerializeField] private BattlePlayerView battlePlayerView;
        [SerializeField] private BattleEnemyViewController battleEnemyViewController;

        private Card currentCard;
        private Action<Card, CardTarget> onCompleteCallback;
        private Queue<CardTargetType> targetTypeQueue = new Queue<CardTargetType>();
        private List<CardTarget> collectedTargets = new List<CardTarget>();

        public void RequestTarget(Card card, Action<Card, CardTarget> activateCard)
        {
            currentCard = card;
            onCompleteCallback = activateCard;
            targetTypeQueue.Clear();
            collectedTargets.Clear();

            if (card.TargetType is CompositeCardTargetType composite)
            {
                foreach (var reqType in composite.RequiredTypes)
                {
                    targetTypeQueue.Enqueue(reqType);
                }
            }
            else
            {
                targetTypeQueue.Enqueue(card.TargetType);
            }

            ProcessNextTargetInQueue();
        }

        private void ProcessNextTargetInQueue()
        {
            if (targetTypeQueue.Count == 0)
            {
                FinishTargeting();
                return;
            }

            CardTargetType nextType = targetTypeQueue.Dequeue();

            CardTarget instantTarget = GetInstantTargetOrNull(nextType);
            if (instantTarget != null)
            {
                collectedTargets.Add(instantTarget);
                ProcessNextTargetInQueue();
                return;
            }

            RequestManualSelect(nextType);
        }

        private void RequestManualSelect(CardTargetType targetType)
        {
            if (targetType is SingleEnemyCardTargetType)
            {
                foreach (var view in battleEnemyViewController.SpawnedEnemyViews)
                {
                    view.OnCardTargetable(OnSingleEnemySelected);
                }
            }
            else if (targetType is BattleEntityCardTargetType)
            {
                battlePlayerView.OnCardTargetable(OnEntitySelected);

                foreach (var view in battleEnemyViewController.SpawnedEnemyViews)
                {
                    view.OnCardTargetable(OnEntitySelected);
                }
            }
            else
            {
                throw new InvalidOperationException($"[CardTargetSelectSystem] {targetType} is not expected to be a manual card target.");
            }
        }

        private void OnSingleEnemySelected(IReadOnlyBattleEnemy enemy)
        {
            ClearAllTargetables(); 
            
            collectedTargets.Add(new SingleEnemyCardTarget(enemy));
            ProcessNextTargetInQueue();
        }

        private void OnEntitySelected(IReadOnlyBattleEntity entity)
        {
            ClearAllTargetables();
            
            collectedTargets.Add(new BattleEntityCardTarget(entity));
            ProcessNextTargetInQueue();
        }

        private void ClearAllTargetables()
        {
            battlePlayerView.OnCardUntargetable();
            foreach (var view in battleEnemyViewController.SpawnedEnemyViews)
            {
                view.OnCardUntargetable();
            }
        }

        private void FinishTargeting()
        {
            CardTarget finalTarget;

            if (currentCard.TargetType is CompositeCardTargetType)
            {
                finalTarget = new CompositeCardTarget(new List<CardTarget>(collectedTargets));
            }
            else
            {
                finalTarget = collectedTargets[0];
            }

            var callback = onCompleteCallback;
            var card = currentCard;
            currentCard = null;
            onCompleteCallback = null;

            callback?.Invoke(card, finalTarget);
        }

        private CardTarget GetInstantTargetOrNull(CardTargetType targetType)
        {
            return targetType switch
            {
                NoneCardTargetType => new NoneCardTarget(),
                PlayerCardTargetType => new PlayerCardTarget(battlePlayerView.Player),
                AllEnemyCardTargetType => new AllEnemyCardTarget(
                    battleEnemyViewController.SpawnedEnemyViews.Select(view => view.Enemy).ToList()
                ),
                _ => null 
            };
        }
    }
}