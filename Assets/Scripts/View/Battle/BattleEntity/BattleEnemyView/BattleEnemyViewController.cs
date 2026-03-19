using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using System.Collections;

namespace View.BattleView
{
    public class BattleEnemyViewController : InteractableViewBehaviour<IBattleViewEvent, IBattleViewCommander>
    {
        [Serializable]
        public struct EnemyPosition
        {
            [SerializeField] private int count;
            [SerializeField] private List<Vector2> positions;

            public int Count => count;
            public List<Vector2> Positions => positions;
        }

        [SerializeField] BattleViewTransitionManager viewTransitionManager;
        [SerializeField] GameObject battleEnemyPrefab;
        [SerializeField] private List<EnemyPosition> enemyPositionConfigs;
        private List<BattleEnemyView> spawnedEnemyViews = new List<BattleEnemyView>();
        private Sequence currentPositionTween;

        public IReadOnlyList<BattleEnemyView> SpawnedEnemyViews { get => spawnedEnemyViews; }

        public override void OnInitialized()
        {
            eventBus.Subscribe<InitialEnemySettled>(OnInitialEnemySettled);
            eventBus.Subscribe<EnemySpawned>(OnEnemySpawned);
            eventBus.Subscribe<EnemyRemoved>(OnEnemyRemoved);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialEnemySettled>(OnInitialEnemySettled);
            eventBus?.Unsubscribe<EnemySpawned>(OnEnemySpawned);
            eventBus?.Unsubscribe<EnemyRemoved>(OnEnemyRemoved);
        }

        private void OnInitialEnemySettled(InitialEnemySettled payload)
        {
            int enemyCount = payload.Enemies.Count;

            EnemyPosition? targetConfig = enemyPositionConfigs.FirstOrDefault(config => config.Count == enemyCount);
            if (targetConfig == null)
            {
                Debug.LogError($"[BattleEnemyViewController/OnInitialEnemySettled] Error: No EnemyPosition configuration found for enemy count ({enemyCount})!");
                return;
            }

            if (targetConfig.Value.Count != targetConfig.Value.Positions.Count)
            {
                Debug.LogError($"[BattleEnemyViewController/OnInitialEnemySettled] Error: EnemyPosition configuration mismatch! The configured Count ({targetConfig.Value.Count}) does not match the actual number of positions ({targetConfig.Value.Positions.Count}).");
                return;
            }

            for (int i = 0; i < enemyCount; i++)
            {
                IReadOnlyBattleEnemy enemy = payload.Enemies[i];
                Vector2 spawnPos = targetConfig.Value.Positions[i];

                GameObject enemyObj = Instantiate(battleEnemyPrefab, transform);

                BattleEnemyView enemyView = enemyObj.GetComponent<BattleEnemyView>();
                if (enemyView != null)
                {
                    enemyView.Initialize(eventBus, presentationManager, commander);
                    enemyView.Initialize(enemy, spawnPos, viewTransitionManager);

                    spawnedEnemyViews.Add(enemyView);
                }
                else
                {
                    Debug.LogError("[BattleEnemyViewController/OnInitialEnemySettled] The given battleEnemyPrefab does not have BattleEnemyView.");
                }
            }
        }

        private void OnEnemySpawned(EnemySpawned payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemySpawned_PositionSet, SpawnEnemyPresentation(payload.Enemy));
        }

        private IEnumerator SpawnEnemyPresentation(IReadOnlyBattleEnemy enemy)
        {
            GameObject enemyObj = Instantiate(battleEnemyPrefab, transform);
            BattleEnemyView enemyView = enemyObj.GetComponent<BattleEnemyView>();

            if (enemyView != null)
            {
                enemyView.Initialize(eventBus, presentationManager, commander);
                
                spawnedEnemyViews.Add(enemyView);

                int newEnemyCount = spawnedEnemyViews.Count;
                Vector2 spawnPos = Vector2.zero;

                EnemyPosition? targetConfig = enemyPositionConfigs.FirstOrDefault(config => config.Count == newEnemyCount);
                
                if (targetConfig != null && targetConfig.Value.Count == targetConfig.Value.Positions.Count)
                {
                    spawnPos = targetConfig.Value.Positions[newEnemyCount - 1];
                }
                else
                {
                    Debug.LogError($"[BattleEnemyViewController/SpawnEnemyPresentation] Error: No valid EnemyPosition configuration found for enemy count ({newEnemyCount})!");
                }

                enemyView.Initialize(enemy, spawnPos, viewTransitionManager);

                if (targetConfig != null)
                {
                    yield return PositioningPresentationRoutine(targetConfig.Value);
                }
            }
            else
            {
                Debug.LogError("[BattleEnemyViewController/SpawnEnemyPresentation] The given battleEnemyPrefab does not have BattleEnemyView.");
            }
        }

        private void OnEnemyRemoved(EnemyRemoved payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyRemoved_PositionSet, RemoveEnemyPresentation(payload.Enemy));
        }

        private IEnumerator RemoveEnemyPresentation(IReadOnlyBattleEnemy enemy)
        {
            BattleEnemyView viewToRemove = spawnedEnemyViews.FirstOrDefault(v => v.Enemy.Equals(enemy));
            
            if (viewToRemove != null)
            {
                spawnedEnemyViews.Remove(viewToRemove);
                Destroy(viewToRemove.gameObject);

                int newEnemyCount = spawnedEnemyViews.Count;
                if (newEnemyCount > 0)
                {
                    EnemyPosition? targetConfig = enemyPositionConfigs.FirstOrDefault(config => config.Count == newEnemyCount);
                    if (targetConfig != null && targetConfig.Value.Count == targetConfig.Value.Positions.Count)
                    {
                        yield return PositioningPresentationRoutine(targetConfig.Value);
                    }
                    else
                    {
                        Debug.LogError($"[BattleEnemyViewController/RemoveEnemyPresentation] Error: No valid EnemyPosition configuration found for enemy count ({newEnemyCount})!");
                    }
                }
            }
            else
            {
                Debug.LogWarning("[BattleEnemyViewController/RemoveEnemyPresentation] Target BattleEnemyView not found in spawnedEnemyViews.");
            }
        }

        private IEnumerator PositioningPresentationRoutine(EnemyPosition config)
        {
            currentPositionTween?.Kill();
            currentPositionTween = DOTween.Sequence();

            for (int i = 0; i < spawnedEnemyViews.Count; i++)
            {
                Vector2 targetPos = config.Positions[i];
                Vector2 currentPos = spawnedEnemyViews[i].transform.position;
                
                if (Vector2.Distance(currentPos, targetPos) > 0.01f)
                {
                    currentPositionTween.Join(spawnedEnemyViews[i].UpdatePosition(targetPos));
                }
            }

            if (currentPositionTween.IsActive() && currentPositionTween.Duration() > 0)
            {
                yield return currentPositionTween.WaitForCompletion();
            }
        }
    }
}