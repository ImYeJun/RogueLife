using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using UnityEngine.UI;
using TMPro;

namespace View.BattleView
{
    public class BattleEnemyView : InteractableViewBehaviour<IBattleViewEvent, IBattleViewCommander>
    {
        private IReadOnlyBattleEnemy enemy;

        [SerializeField] private SpriteRenderer image;
        [SerializeField] private Image healthBar;
        [SerializeField] private TextMeshProUGUI healthText;

        public override void OnInitialized()
        {
            eventBus.Subscribe<EnemyActionPlanned>(OnEnemyActionPlanned);
        }

        public override void OnDestroy()
        {
            eventBus.Unsubscribe<EnemyActionPlanned>(OnEnemyActionPlanned);
        }

        public void Initialize(IReadOnlyBattleEnemy enemy, Vector3 spawnPos)
        {
            this.enemy = enemy;
            transform.position = spawnPos;
            image.sprite = enemy.Data.GetBattleSprite(EnemySpriteType.Idle);

            DrawHealthBar();
        }

        public void OnEnemyActionPlanned(EnemyActionPlanned payload)
        {
            if (payload.Enemy != enemy) { return; }

            foreach (var action in enemy.PlannedActions)
            {
                Debug.Log(action);
            }
        }

        private void DrawHealthBar()
        {
            healthBar.fillAmount = enemy.NormalizedHealth;
            healthText.text = $"{enemy.CurrentHealth}/{enemy.MaxHealth}";
        }
    }
}