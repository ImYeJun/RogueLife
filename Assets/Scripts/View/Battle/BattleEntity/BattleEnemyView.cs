using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace View.BattleView
{
    public class BattleEnemyView : BattleEntityView<IReadOnlyBattleEnemy>
    {
        private const float ICONS_CONAINTER_HEIGHT = 1;

        private IReadOnlyBattleEnemy enemy;

        [Header("BattleEnemyView")]
        [SerializeField] private Image healthBar;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private GameObject actionIconPrefab;
        [SerializeField] private RectTransform iconsContainer;
        private List<BattleEnemyActionIcon> actionIcons = new List<BattleEnemyActionIcon>();

        public IReadOnlyBattleEnemy Enemy { get => enemy; }

        public override void OnInitialized()
        {
            base.OnInitialized();
            actionIcons.Clear();
            iconsContainer.sizeDelta = new Vector2(1, ICONS_CONAINTER_HEIGHT);

            eventBus.Subscribe<EnemyActionPlanned>(OnEnemyActionPlanned);
            eventBus.Subscribe<EnemyHurt>(OnEnemyHurt);
            eventBus.Subscribe<EnemyHealed>(OnEnemyHealed);
            eventBus.Subscribe<EnemyDied>(OnEnemyDied);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            eventBus?.Unsubscribe<EnemyActionPlanned>(OnEnemyActionPlanned);
            eventBus?.Unsubscribe<EnemyHurt>(OnEnemyHurt);
            eventBus?.Unsubscribe<EnemyHealed>(OnEnemyHealed);
            eventBus?.Unsubscribe<EnemyDied>(OnEnemyDied);
        }

        public void Initialize(IReadOnlyBattleEnemy enemy, Vector3 spawnPos)
        {
            this.enemy = enemy;
            entity = enemy;

            transform.position = spawnPos;
            spriteRenderer.sprite = enemy.Data.GetBattleSprite(EnemySpriteType.Idle);

            DrawHealthBar(enemy.CurrentHealth, enemy.MaxHealth);
        }

        public void OnEnemyActionPlanned(EnemyActionPlanned payload)
        {
            if (enemy == null)
            {
                throw new InvalidOperationException("[BattleEnemyView] The enemy entity is not initialized yet.");
            }

            if (!payload.Enemy.Equals(enemy)) { return; }

            foreach (var icon in actionIcons)
            {
                Destroy(icon.gameObject);
            }
            actionIcons.Clear();

            iconsContainer.sizeDelta = new Vector2(enemy.PlannedActions.Count, ICONS_CONAINTER_HEIGHT);
            foreach (var action in enemy.PlannedActions)
            {
                var iconObject = Instantiate(actionIconPrefab, iconsContainer);

                var actionIcon = iconObject.GetComponent<BattleEnemyActionIcon>();
                actionIcon.Initialize(action);

                actionIcons.Add(actionIcon);
            }
        }

        private void OnEnemyHurt(EnemyHurt payload)
        {
            if (enemy == null)
            {
                throw new InvalidOperationException("[BattleEnemyView] The enemy entity is not initialized yet.");
            }

            if (!payload.Enemy.Equals(enemy)) { return; }

            DrawHealthBar(payload.CurrentHealth, enemy.MaxHealth);
        }

        private void OnEnemyHealed(EnemyHealed payload)
        {
            if (enemy == null)
            {
                throw new InvalidOperationException("[BattleEnemyView] The enemy entity is not initialized yet.");
            }

            if (!payload.Enemy.Equals(enemy)) { return; }

            DrawHealthBar(payload.CurrentHealth, enemy.MaxHealth);
        }

        private void OnEnemyDied(EnemyDied payload)
        {
            if (enemy == null)
            {
                throw new InvalidOperationException("[BattleEnemyView] The enemy entity is not initialized yet.");
            }

            if (!payload.DiedEnemy.Equals(enemy)) { return; }
            actionIcons.Clear();
            Destroy(gameObject); 
        }

        private void DrawHealthBar(int currentHealth, int maxHealth)
        {
            float normalizedHealth = maxHealth == 0 ? 0 : (float)currentHealth / maxHealth;
            healthBar.fillAmount = normalizedHealth;
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}