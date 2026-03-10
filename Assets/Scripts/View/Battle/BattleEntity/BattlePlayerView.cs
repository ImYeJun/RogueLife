using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using UnityEngine.UI;
using TMPro;
using System;

namespace View.BattleView
{
    public class BattlePlayerView : ViewBehaviour<IBattleViewEvent>
    {
        private IReadOnlyBattlePlayer player;

        [SerializeField] private Vector2 initialPos;
        [SerializeField] private Image battleHeatlhBar;
        [SerializeField] private TextMeshProUGUI battleHeatlhText;
        [SerializeField] private Image mentalityBar;
        [SerializeField] private TextMeshProUGUI mentaltiyText;

        public override void OnInitialized()
        {
            eventBus.Subscribe<PlayerSettled>(OnPlayerSettled);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<PlayerSettled>(OnPlayerSettled);
        }

        public void OnPlayerSettled(PlayerSettled payload)
        {
            transform.position = initialPos;

            player = payload.Player;
            
            DrawBattleHealthBar();
            DrawMentalityBar();
        }

        private void DrawBattleHealthBar()
        {
            battleHeatlhBar.fillAmount = player.Health.NormalizedBattleHealth;
            battleHeatlhText.text = $"{player.Health.CurrentBattleHealth}/{player.Health.MaxBattleHealth}";
        }

        private void DrawMentalityBar()
        {
            mentalityBar.fillAmount = player.Health.NomarlizedMentality;
            mentaltiyText.text = $"{player.Health.CurrentMentality}/{player.Health.MaxMentality}";
        }
    }
}
