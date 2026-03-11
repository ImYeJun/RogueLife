using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using UnityEngine.UI;
using TMPro;
using System;

namespace View.BattleView
{
    public class BattlePlayerView : BattleEntityView<IReadOnlyBattlePlayer>
    {
        private IReadOnlyBattlePlayer player;

        [Header("BattlePlayerView")]
        [SerializeField] private Vector2 initialPos;
        [SerializeField] private Image battleHeatlhBar;
        [SerializeField] private TextMeshProUGUI battleHeatlhText;
        [SerializeField] private Image mentalityBar;
        [SerializeField] private TextMeshProUGUI mentaltiyText;

        public IReadOnlyBattlePlayer Player { get => player; }

        public override void OnInitialized()
        {
            base.OnInitialized();
            eventBus.Subscribe<PlayerSettled>(OnPlayerSettled);
            eventBus.Subscribe<PlayerHurt>(OnPlayerHurt);
            eventBus.Subscribe<PlayerHealed>(OnPlayerHealed);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            eventBus?.Unsubscribe<PlayerSettled>(OnPlayerSettled);
            eventBus?.Unsubscribe<PlayerHurt>(OnPlayerHurt);
            eventBus?.Unsubscribe<PlayerHealed>(OnPlayerHealed);
        }

        public void OnPlayerSettled(PlayerSettled payload)
        {
            transform.position = initialPos;

            player = payload.Player;
            entity = player;
            
            DrawBattleHealthBar(player.Health.CurrentBattleHealth, player.Health.MaxBattleHealth);
            DrawMentalityBar(player.Health.CurrentMentality, player.Health.MaxMentality);
        }

        private void OnPlayerHurt(PlayerHurt payload)
        {
            if (player == null)
            {
                throw new InvalidOperationException("[BattlePlayerView] The player entity is not initialized yet.");
            }

            if (!payload.Player.Equals(player)) { return; }

            DrawBattleHealthBar(payload.CurrentBattleHealth, player.Health.MaxBattleHealth);
            DrawMentalityBar(payload.CurrentMentality, player.Health.MaxMentality);
        }

        private void OnPlayerHealed(PlayerHealed payload)
        {
            if (player == null)
            {
                throw new InvalidOperationException("[BattlePlayerView] The player entity is not initialized yet.");
            }

            if (!payload.Player.Equals(player)) { return; }

            DrawBattleHealthBar(payload.CurrentBattleHealth, player.Health.MaxBattleHealth);
        }

        private void DrawBattleHealthBar(int currentHealth, int maxHealth)
        {
            float normalizedHealth = maxHealth == 0 ? 0 : (float)currentHealth / maxHealth;
            battleHeatlhBar.fillAmount = normalizedHealth;
            battleHeatlhText.text = $"{currentHealth}/{maxHealth}";
        }

        private void DrawMentalityBar(int currentMentality, int maxMentality)
        {
            float normalizedMentality = maxMentality == 0 ? 0 : (float)currentMentality / maxMentality;
            mentalityBar.fillAmount = normalizedMentality;
            mentaltiyText.text = $"{currentMentality}/{maxMentality}";
        }
    }
}