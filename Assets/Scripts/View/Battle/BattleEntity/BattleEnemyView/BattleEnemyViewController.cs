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
        }

        public override void OnDestroy()
        {
            eventBus.Unsubscribe<InitialEnemySettled>(OnInitialEnemySettled);
        }


        private void OnInitialEnemySettled(InitialEnemySettled payload)
        {
            int enemyCount = payload.Enemies.Count;

            EnemyPosition? targetConfig = enemyPositionConfigs.FirstOrDefault(config => config.Count == enemyCount);
            if (targetConfig == null)
            {
                Debug.LogError($"[BattleEnemyViewController] Error: No EnemyPosition configuration found for enemy count ({enemyCount})!");
                return;
            }

            if (targetConfig.Value.Count != targetConfig.Value.Positions.Count)
            {
                Debug.LogError($"[BattleEnemyViewController] Error: EnemyPosition configuration mismatch! The configured Count ({targetConfig.Value.Count}) does not match the actual number of positions ({targetConfig.Value.Positions.Count}).");
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
                    enemyView.Initialize(eventBus, presentationManager);
                    enemyView.Initialize(enemy, spawnPos, viewTransitionManager);

                    spawnedEnemyViews.Add(enemyView);
                }
                else
                {
                    Debug.LogError("[BattleEnemyViewController] The given battleEnemyPrefab does not have BattleEnemyView.");
                }
            }
        }
    }
}