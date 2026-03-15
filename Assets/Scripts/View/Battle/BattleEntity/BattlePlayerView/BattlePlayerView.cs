using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Linq;
using DG.Tweening;

namespace View.BattleView
{
    public class BattlePlayerView : BattleEntityView<IReadOnlyBattlePlayer>
    {
        private PlayerHurtPresentation hurtPresentation;

        private IReadOnlyBattlePlayer player;
        private BattlePlayerActionPresentation actionPresentation;
        private BattlePlayerBodyView bodyView;
        private int currentBattleHealth;

        private int currentMentality;


        [Header("BattlePlayerView")]
        [SerializeField] private Vector2 initialPos;
        [SerializeField] private Transform healthBar;
        [SerializeField] private Image battleHeatlhBar;
        [SerializeField] private TextMeshProUGUI battleHeatlhText;
        [SerializeField] private Image mentalityBar;
        [SerializeField] private TextMeshProUGUI mentaltiyText;
        [SerializeField] private TextMeshProUGUI actionText;

        [Header("Heal Presentation Settings")]
        [Tooltip("Settings for battle health heal.")]
        [SerializeField] private float healDuration = 0.3f;
        [SerializeField] private float healTextOffsetDuration = 0.2f;
        [SerializeField] private Ease healEase = Ease.OutQuad;

        [Header("Test Only")]
        [SerializeField] private int testMaxHealth = 100;
        [SerializeField] private int testStartHealth = 30;
        [SerializeField] private int testHealAmount = 25;

        public IReadOnlyBattlePlayer Player { get => player; }

        private void Awake() {
            actionPresentation = GetComponent<BattlePlayerActionPresentation>();
            bodyView = GetComponentInChildren<BattlePlayerBodyView>();
            hurtPresentation = GetComponent<PlayerHurtPresentation>();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();

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

            currentBattleHealth = player.Health.CurrentBattleHealth;
            currentMentality = player.Health.CurrentMentality;
            
            actionPresentation.Initialize(player, presentationManager, PlayActionPresentation);
            hurtPresentation.Initialize(transform, healthBar, mentalityBar, mentaltiyText, battleHeatlhBar, battleHeatlhText);

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
            

            currentBattleHealth = payload.CurrentBattleHealth;
            currentMentality = payload.CurrentMentality;
            int snapshotedBattleHealth = currentBattleHealth;
            int snapshotedMentality = currentMentality;

            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.PlayerHurt_PlayerPresentation, 
                hurtPresentation.Play(payload, currentBattleHealth, currentMentality, battleStatusEffectIconContainer.Cast<Transform>().ToList()),
                () =>
                {
                    DrawBattleHealthBar(snapshotedBattleHealth, player.Health.MaxBattleHealth);
                    DrawMentalityBar(snapshotedMentality, player.Health.MaxMentality);
                }
            );
        }

        private void OnPlayerHealed(PlayerHealed payload)
        {
            if (player == null)
            {
                throw new InvalidOperationException("[BattlePlayerView/OnPlayerHealed] The player entity is not initialized yet.");
            }

            if (!payload.Player.Equals(player)) { return; }

            int startHealth = currentBattleHealth;
            int targetHealth = payload.CurrentBattleHealth;

            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.PlayerHeal_HealthBarPresentation, 
                UpdateHealHealthBarPresentation(startHealth, targetHealth, player.Health.MaxBattleHealth),
                () =>
                {
                    DrawBattleHealthBar(targetHealth, player.Health.MaxBattleHealth);
                }
            );

            currentBattleHealth = targetHealth;
        }
        
        private IEnumerator UpdateHealHealthBarPresentation(int startHealth, int targetHealth, int maxHealth)
        {
            float targetNormalized = maxHealth == 0 ? 0 : (float)targetHealth / maxHealth;
            float totalDuration = healDuration + healTextOffsetDuration;

            Sequence sequence = DOTween.Sequence();

            sequence.Join(battleHeatlhBar.DOFillAmount(targetNormalized, healDuration).SetEase(healEase));

            int tempHealth = startHealth;
            sequence.Join(DOTween.To(
                () => tempHealth,
                (val) =>
                {
                    tempHealth = val;
                    battleHeatlhText.text = $"{tempHealth}/{maxHealth}";
                },
                targetHealth,
                totalDuration
            ).SetEase(healEase));

            yield return sequence.WaitForCompletion();
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

        public IEnumerator PlayActionPresentation()
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

        [ContextMenu("Test Heal Presentation")]
        public void TestHealPresentation()
        {
            int maxH = player != null ? player.Health.MaxBattleHealth : testMaxHealth;
            DrawBattleHealthBar(testStartHealth, maxH);

            int targetHealth = Mathf.Min(maxH, testStartHealth + testHealAmount);
            StartCoroutine(DelayTestHealPresentation(testStartHealth, targetHealth, maxH));
        }

        private IEnumerator DelayTestHealPresentation(int startHealth, int targetHealth, int maxHealth)
        {
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(UpdateHealHealthBarPresentation(startHealth, targetHealth, maxHealth));
        }
    }
}