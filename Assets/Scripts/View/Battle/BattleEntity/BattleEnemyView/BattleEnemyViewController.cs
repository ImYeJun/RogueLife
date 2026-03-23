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
        public struct EnemyPositionConfig
        {
            [Serializable]
            public struct SpawnPoint
            {
                public float x;
                public float y;
                public int spriteSortOrder;
                
                public Vector2 Coordinate => new Vector2(x, y);
            }

            [SerializeField] private int count;
            [SerializeField] private List<SpawnPoint> spawnPoints;

            public int Count => count;
            public List<SpawnPoint> SpawnPoints => spawnPoints;
        }

        [SerializeField] BattleViewTransitionManager viewTransitionManager;
        [SerializeField] GameObject battleEnemyPrefab;
        [SerializeField] private List<EnemyPositionConfig> enemyPositionConfigs;
        
        private List<BattleEnemyView> spawnedEnemyViews = new List<BattleEnemyView>();
        private Sequence currentPositionTween;

        public IReadOnlyList<BattleEnemyView> SpawnedEnemyViews => spawnedEnemyViews;

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

            EnemyPositionConfig? targetConfig = enemyPositionConfigs.FirstOrDefault(config => config.Count == enemyCount);
            if (targetConfig == null)
            {
                Debug.LogError($"[BattleEnemyViewController/OnInitialEnemySettled] Error: No EnemyPosition configuration found for enemy count ({enemyCount})!");
                return;
            }

            if (targetConfig.Value.Count != targetConfig.Value.SpawnPoints.Count)
            {
                Debug.LogError($"[BattleEnemyViewController/OnInitialEnemySettled] Error: EnemyPosition configuration mismatch! The configured Count ({targetConfig.Value.Count}) does not match the actual number of positions ({targetConfig.Value.SpawnPoints.Count}).");
                return;
            }

            for (int i = 0; i < enemyCount; i++)
            {
                IReadOnlyBattleEnemy enemy = payload.Enemies[i];
                Vector2 spawnPos = targetConfig.Value.SpawnPoints[i].Coordinate;

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
            IReadOnlyBattleEnemy enemy = payload.Enemy;
            GameObject enemyObj = Instantiate(battleEnemyPrefab, transform);
            BattleEnemyView enemyView = enemyObj.GetComponent<BattleEnemyView>();

            if (enemyView != null)
            {
                enemyView.Initialize(eventBus, presentationManager, commander);
                enemyView.SetInvisibleDirectly();

                spawnedEnemyViews.Add(enemyView);

                int newEnemyCount = spawnedEnemyViews.Count;
                Vector2 spawnPos = Vector2.zero;

                EnemyPositionConfig? targetConfig = enemyPositionConfigs.FirstOrDefault(config => config.Count == newEnemyCount);
                if (targetConfig != null && targetConfig.Value.Count == targetConfig.Value.SpawnPoints.Count)
                {
                    spawnPos = targetConfig.Value.SpawnPoints[newEnemyCount - 1].Coordinate;
                }
                else
                {
                    Debug.LogError($"[BattleEnemyViewController/OnEnemySpawned] Error: No valid EnemyPosition configuration found for enemy count ({newEnemyCount})!");
                }

                enemyView.Initialize(enemy, spawnPos, viewTransitionManager);

                var snapshot = new List<BattleEnemyView>(spawnedEnemyViews);

                presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemySpawned_PositionSet, SpawnEnemyPresentation(enemyView, targetConfig, snapshot));
            }
            else
            {
                Debug.LogError("[BattleEnemyViewController/OnEnemySpawned] The given battleEnemyPrefab does not have BattleEnemyView.");
            }
        }

        private IEnumerator SpawnEnemyPresentation(BattleEnemyView newlySpawnedView, EnemyPositionConfig? targetConfig, List<BattleEnemyView> snapshot)
        {
            if (targetConfig != null)
            {
                currentPositionTween?.Kill();
                currentPositionTween = DOTween.Sequence();

                for (int i = 0; i < snapshot.Count; i++)
                {
                    if (snapshot[i] == null) continue;

                    var spawnPoint = targetConfig.Value.SpawnPoints[i];
                    Vector2 targetPos = spawnPoint.Coordinate;
                    Vector2 currentPos = snapshot[i].transform.position;
                    
                    if (Vector2.Distance(currentPos, targetPos) > 0.01f)
                    {
                        currentPositionTween.Join(snapshot[i].UpdatePosition(targetPos, spawnPoint.spriteSortOrder));
                    }
                }

                currentPositionTween.Join(newlySpawnedView.PlayAppearPresentation());

                if (currentPositionTween.IsActive() && currentPositionTween.Duration() > 0)
                {
                    yield return currentPositionTween.WaitForCompletion();
                }
            }
        }

        private void OnEnemyRemoved(EnemyRemoved payload)
        {
            BattleEnemyView viewToRemove = spawnedEnemyViews.FirstOrDefault(v => v.Enemy.Equals(payload.Enemy));
            
            if (viewToRemove != null)
            {
                spawnedEnemyViews.Remove(viewToRemove);

                var snapshot = new List<BattleEnemyView>(spawnedEnemyViews);
                int newEnemyCount = snapshot.Count;
                
                EnemyPositionConfig? targetConfig = null;
                if (newEnemyCount > 0)
                {
                    targetConfig = enemyPositionConfigs.FirstOrDefault(config => config.Count == newEnemyCount);
                }

                presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyRemoved_PositionSet, RemoveEnemyPresentation(viewToRemove, targetConfig, snapshot));
            }
            else
            {
                Debug.LogWarning("[BattleEnemyViewController/OnEnemyRemoved] Target BattleEnemyView not found in spawnedEnemyViews.");
            }
        }

        private IEnumerator RemoveEnemyPresentation(BattleEnemyView viewToRemove, EnemyPositionConfig? targetConfig, List<BattleEnemyView> snapshot)
        {
            if (viewToRemove != null)
            {
                yield return viewToRemove.PlayDisappearPresentation().WaitForCompletion();
                Destroy(viewToRemove.gameObject);
            }

            if (targetConfig != null && targetConfig.Value.Count == targetConfig.Value.SpawnPoints.Count)
            {
                yield return PositioningPresentationRoutine(targetConfig.Value, snapshot);
            }
        }

        private IEnumerator PositioningPresentationRoutine(EnemyPositionConfig config, List<BattleEnemyView> snapshot)
        {
            currentPositionTween?.Kill();
            currentPositionTween = DOTween.Sequence();

            for (int i = 0; i < snapshot.Count; i++)
            {
                if (snapshot[i] == null) continue;

                var spawnPoint = config.SpawnPoints[i];
                Vector2 targetPos = spawnPoint.Coordinate;
                Vector2 currentPos = snapshot[i].transform.position;
                
                if (Vector2.Distance(currentPos, targetPos) > 0.01f)
                {
                    currentPositionTween.Join(snapshot[i].UpdatePosition(targetPos, spawnPoint.spriteSortOrder)); 
                }
            }

            if (currentPositionTween.IsActive() && currentPositionTween.Duration() > 0)
            {
                yield return currentPositionTween.WaitForCompletion();
            }
        }
    }
}