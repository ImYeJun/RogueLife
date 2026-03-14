using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

namespace View.BattleView
{
    public class BattlePlayerView : BattleEntityView<IReadOnlyBattlePlayer>
    {
        private IReadOnlyBattlePlayer player;
        private BattlePlayerActionPresentation actionPresentation;
        private BattlePlayerBodyView bodyView;


        [Header("BattlePlayerView")]
        [SerializeField] private Vector2 initialPos;
        [SerializeField] private Image battleHeatlhBar;
        [SerializeField] private TextMeshProUGUI battleHeatlhText;
        [SerializeField] private Image mentalityBar;
        [SerializeField] private TextMeshProUGUI mentaltiyText;
        [SerializeField] private TextMeshProUGUI actionText;

        public IReadOnlyBattlePlayer Player { get => player; }

        public override void OnInitialized()
        {
            base.OnInitialized();
            actionPresentation = GetComponent<BattlePlayerActionPresentation>();
            bodyView = GetComponentInChildren<BattlePlayerBodyView>();


            eventBus.Subscribe<PlayerSettled>(OnPlayerSettled);
            eventBus.Subscribe<PlayerHurt>(OnPlayerHurt);
            eventBus.Subscribe<PlayerHealed>(OnPlayerHealed);
            eventBus.Subscribe<CardEffectExecuted>(actionPresentation.OnCardEffectExecuted);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            eventBus?.Unsubscribe<PlayerSettled>(OnPlayerSettled);
            eventBus?.Unsubscribe<PlayerHurt>(OnPlayerHurt);
            eventBus?.Unsubscribe<PlayerHealed>(OnPlayerHealed);
            eventBus?.Unsubscribe<CardEffectExecuted>(actionPresentation.OnCardEffectExecuted);
        }

        public void OnPlayerSettled(PlayerSettled payload)
        {
            transform.position = initialPos;

            player = payload.Player;
            bodyView.Initialize(player, this, viewTransitionManager.InspectEntity, BattleEntityInspectorView.InspectorDirection.Right);
            entity = player;
            
            actionPresentation.Initiate(player, presentationManager, PlayActionPresentation);
            DrawBattleHealthBar(player.Health.CurrentBattleHealth, player.Health.MaxBattleHealth);
            DrawMentalityBar(player.Health.CurrentMentality, player.Health.MaxMentality);
        }

        private void OnPlayerHurt(PlayerHurt payload)
        {
            if (player == null)
            {
                throw new InvalidOperationException("[BattlePlayerView/OnPlayerHurt] The player entity is not initialized yet.");
            }

            if (!payload.Player.Equals(player)) { return; }

            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.PlayerHurt_HealthBarPresentation, 
                UpdateHurtHealthBarPresentation(payload.CurrentBattleHealth, payload.CurrentMentality, player.Health.MaxBattleHealth, player.Health.MaxMentality)
            );
            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.PlayerHurt_HealthBarPresentation, 
                PlayHurtPresentation()
            );
        }

        private void OnPlayerHealed(PlayerHealed payload)
        {
            if (player == null)
            {
                throw new InvalidOperationException("[BattlePlayerView/OnPlayerHealed] The player entity is not initialized yet.");
            }

            if (!payload.Player.Equals(player)) { return; }

            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.PlayerHeal_HealthBarPresentation, 
                UpdateHealHealthBarPresentation(payload.CurrentBattleHealth, player.Health.MaxBattleHealth)
            );
        }

        private IEnumerator UpdateHurtHealthBarPresentation(int currentHealth, int currentMentality, int maxHealth, int maxMentality)
        {
            DrawBattleHealthBar(currentHealth, maxHealth);
            DrawMentalityBar(currentMentality, maxMentality);
            yield return null;
        }

        private IEnumerator UpdateHealHealthBarPresentation(int currentHealth, int maxHealth)
        {
            DrawBattleHealthBar(currentHealth, maxHealth);
            yield return null;
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

        public override IEnumerator PlayHurtPresentation()
        {
            actionText.text = "아야 아파요";
            yield return new WaitForSeconds(1.0f);
            actionText.text = "";
        }
        
        public override IEnumerator PlayActionPresentation()
        {
            actionText.text = "행동함";
            yield return new WaitForSeconds(1.0f);
            actionText.text = "";
        }

        public override void OnInspect(IInspectorBuilder builder, RectTransform parent)
        {
            var nameText = builder.AddNameText(parent);
            nameText.Text = "유지아";

            base.OnInspect(builder, parent);
        }
    }
}