using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private void UpdateEnemyPositions()
        {
            int enemyCount = spawnedEnemyViews.Count;
            if (enemyCount == 0) return;

            EnemyPosition? targetConfig = enemyPositionConfigs.FirstOrDefault(config => config.Count == enemyCount);
            if (targetConfig == null)
            {
                Debug.LogError($"[BattleEnemyViewController/UpdateEnemyPositions] Error: No EnemyPosition configuration found for enemy count ({enemyCount})!");
                return;
            }

            if (targetConfig.Value.Count != targetConfig.Value.Positions.Count)
            {
                Debug.LogError($"[BattleEnemyViewController/UpdateEnemyPositions] Error: EnemyPosition configuration mismatch! The configured Count ({targetConfig.Value.Count}) does not match the actual number of positions ({targetConfig.Value.Positions.Count}).");
                return;
            }

            for (int i = 0; i < enemyCount; i++)
            {
                Vector2 targetPos = targetConfig.Value.Positions[i];
                spawnedEnemyViews[i].UpdatePosition(targetPos);
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
                enemyView.Initialize(enemy, Vector2.zero, viewTransitionManager);

                spawnedEnemyViews.Add(enemyView);
                UpdateEnemyPositions();
            }
            else
            {
                Debug.LogError("[BattleEnemyViewController/OnEnemySpawned] The given battleEnemyPrefab does not have BattleEnemyView.");
            }
        }

        private void OnEnemyRemoved(EnemyRemoved payload)
        {
            BattleEnemyView viewToRemove = spawnedEnemyViews.FirstOrDefault(v => v.Enemy.Equals(payload.Enemy));
            
            if (viewToRemove != null)
            {
                spawnedEnemyViews.Remove(viewToRemove);
                UpdateEnemyPositions();
            }
            else
            {
                Debug.LogWarning("[BattleEnemyViewController/OnEnemyRemoved] Target BattleEnemyView not found in spawnedEnemyViews.");
            }
        }
    }
}