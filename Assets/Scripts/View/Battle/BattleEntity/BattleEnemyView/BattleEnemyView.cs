using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.Serialization;
using System.Linq;

namespace View.BattleView
{
    public class BattleEnemyView : BattleEntityView<IReadOnlyBattleEnemy>
    {
        private const float ICONS_CONAINTER_HEIGHT = 1;

        private IReadOnlyBattleEnemy enemy;
        private BattleEnemyBodyView bodyView;
        private int currentHealth;

        [Header("BattleEnemyView")]
        [SerializeField, FormerlySerializedAs("healthBar")] private Image healthBarImage;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private GameObject actionIconPrefab;
        [SerializeField] private RectTransform actionIconView;
        [SerializeField] private RectTransform actionIconsContainer;
        [SerializeField] private TextMeshProUGUI actionText;
        private List<BattleEnemyActionIcon> actionIcons = new List<BattleEnemyActionIcon>();

        [Header("Hurt Presentation Settings")]
        [SerializeField, Range(0, 1f)] private float heavyHurtRatio;
        [SerializeField] private float normalHurtDuration;
        [SerializeField] private float heavyHurtDuration;
        [SerializeField] private float normalHurtTextOffsetDuration;
        [SerializeField] private float heavyHurtTextOffsetDuration;
        [SerializeField] private Vector3 normalHurShakeAmount;
        [SerializeField] private Vector3 heavyHurtShakeAmount;
        [SerializeField] private int normalHurtShakeVibartor = 10;
        [SerializeField] private int heavyHurtShakeVibartor = 10;
        [SerializeField] private float normalHurtShakeRandomNess = 90;
        [SerializeField] private float heavyHurtShakeRandomNess = 90;
        [SerializeField] private ShakeRandomnessMode normalHurtShakeRandomnessMode = ShakeRandomnessMode.Full;
        [SerializeField] private ShakeRandomnessMode heavyHurtShakeRandomnessMode = ShakeRandomnessMode.Full;
        [SerializeField] private Ease normalHurtEase;
        [SerializeField] private Ease heavyHurtEase;

        [Header("Follow-Through Settings")]
        [Tooltip("본체의 흔들림이 멈춘 후, 부속품들이 흔들리는 여운 시간")]
        [SerializeField] private float normalFollowThroughDuration;
        [SerializeField] private float heavyFollowThroughDuration;
        [SerializeField] private Vector3 normalHealthBarShakeAmount;
        [SerializeField] private Vector3 heavyHealthBarShakeAmount;
        [SerializeField] private Vector3 normalActionIconShakeAmount;
        [SerializeField] private Vector3 heavyActionIconShakeAmount;
        [SerializeField] private Vector3 normalStatusIconShakeAmount;
        [SerializeField] private Vector3 heavyStatusIconShakeAmount;

        [Header("Test Only")]
        [SerializeField] private int testMaxHealth = 100;
        [SerializeField] private int testStartHealth = 100;
        [SerializeField] private int testDamage = 30;

        public IReadOnlyBattleEnemy Enemy { get => enemy; }
        public BattleEnemyBodyView BodyView { get => bodyView; }

        public override void OnInitialized()
        {
            base.OnInitialized();
            actionIcons.Clear();
            actionIconView.sizeDelta = new Vector2(1, ICONS_CONAINTER_HEIGHT);
            bodyView = GetComponentInChildren<BattleEnemyBodyView>();

            eventBus.Subscribe<EnemyActionPlanned>(OnEnemyActionPlanned);
            eventBus.Subscribe<EnemyTurnEnded>(OnEnemyTurnEndend);
            eventBus.Subscribe<EnemyHurt>(OnEnemyHurt);
            eventBus.Subscribe<EnemyHealed>(OnEnemyHealed);
            eventBus.Subscribe<EnemyDied>(OnEnemyDied);
            eventBus.Subscribe<EnemyActionExecuted>(OnEnemyActionExecuted);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            eventBus?.Unsubscribe<EnemyActionPlanned>(OnEnemyActionPlanned);
            eventBus?.Unsubscribe<EnemyTurnEnded>(OnEnemyTurnEndend);
            eventBus?.Unsubscribe<EnemyHurt>(OnEnemyHurt);
            eventBus?.Unsubscribe<EnemyHealed>(OnEnemyHealed);
            eventBus?.Unsubscribe<EnemyDied>(OnEnemyDied);
            eventBus?.Unsubscribe<EnemyActionExecuted>(OnEnemyActionExecuted);
        }

        public void Initialize(IReadOnlyBattleEnemy enemy, Vector3 spawnPos, BattleViewTransitionManager viewTransitionManager)
        {
            this.viewTransitionManager = viewTransitionManager;
            this.enemy = enemy;
            bodyView.Initialize(enemy, this, viewTransitionManager.InspectEntity, BattleEntityInspectorView.InspectorDirection.Left);
            entity = enemy;

            transform.position = spawnPos;

            DrawHealthBarDirectly(enemy.CurrentHealth, enemy.MaxHealth);
        }

        public void OnEnemyActionPlanned(EnemyActionPlanned payload)
        {
            if (enemy == null)
            {
                throw new InvalidOperationException("[BattleEnemyView] The enemy entity is not initialized yet.");
            }

            if (!payload.Enemy.Equals(enemy)) { return; }
            actionIcons.Clear();

            int actionCount = enemy.PlannedActions.Count;
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyActionPlanned_BaseIconAction, PlayActionPlannedPresentation(actionCount),
                () =>
                {
                    actionIconsContainer.sizeDelta = new Vector2(actionCount, ICONS_CONAINTER_HEIGHT);
                } );
            
            for (int i = 0; i < actionCount; i++)
            {
                var action = enemy.PlannedActions[i];
                var iconObject = Instantiate(actionIconPrefab, actionIconsContainer);
                var actionIcon = iconObject.GetComponent<BattleEnemyActionIcon>();
                actionIcon.Initialize(action);
                actionIcons.Add(actionIcon);

                presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyActionPlanned_BaseIconAction + i, actionIcon.PlayAppliedPresentation());
            }
        }

        private IEnumerator PlayActionPlannedPresentation(int actionCount)
        {
            actionIconsContainer.sizeDelta = new Vector2(actionCount, ICONS_CONAINTER_HEIGHT);
            yield return null;
        }

        private void OnEnemyActionExecuted(EnemyActionExecuted payload)
        {
            if (payload.Actor != enemy) { return; }

            var actionView = actionIcons.FirstOrDefault(view => view.Action == payload.Action && !view.HasExecuted);
            //* Notice that enemy action reference is not cloned but same reference.

            if (actionView is null)
            {
                throw new InvalidOperationException($"[{GetType()}]] The Enemy doesn't have given action");
            }

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyActionExecuted_ActorAction, actionView.PlayExecutedPresentation());
            actionView.HasExecuted = true;
        }

        private void OnEnemyTurnEndend(EnemyTurnEnded payload)
        {
            for (int i = actionIcons.Count - 1; i >= 0; i--)
            {
                var actionIcon = actionIcons[i];
                presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyTurnEnded_ActionClear, actionIcon.PlayRemovedPresentation(),
                () =>
                {
                    Destroy(actionIcon.gameObject);
                }
                );
            }
        }

        private void OnEnemyHurt(EnemyHurt payload)
        {
            if (enemy == null)
            {
                throw new InvalidOperationException("[BattleEnemyView] The enemy entity is not initialized yet.");
            }

            if (!payload.Enemy.Equals(enemy)) { return; }

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyHurt_EnemyPresentation, PlayHurtPresentation());
            
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyHurt_HealthBarPresentation, HurtPresentation(payload.Damage, payload.CurrentHealth));
        }

        private void OnEnemyHealed(EnemyHealed payload)
        {
            if (enemy == null)
            {
                throw new InvalidOperationException("[BattleEnemyView] The enemy entity is not initialized yet.");
            }

            if (!payload.Enemy.Equals(enemy)) { return; }

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyHeal_HealthBarPresentation, HealPresentation(payload.HealAmount, payload.CurrentHealth));
        }

        private void OnEnemyDied(EnemyDied payload)
        {
            if (enemy == null)
            {
                throw new InvalidOperationException("[BattleEnemyView] The enemy entity is not initialized yet.");
            }

            if (!payload.DiedEnemy.Equals(enemy)) { return; }
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyDied_DiePresentation, EnemyDiedPresentation(), () =>
            {
                actionIcons.Clear();
                Destroy(gameObject); 
            });
        }
        private IEnumerator EnemyDiedPresentation()
        {
            yield return null;
        }

        private IEnumerator ActionExecutedPresentation(EnemyActionExecuted payload)
        {
            yield return StartCoroutine(PlayActionPresentation());
        }


        private void DrawHealthBarDirectly(int newHealth, int maxHealth)
        {
            float normalizedHealth = maxHealth == 0 ? 0 : (float)newHealth / maxHealth;
            healthBarImage.fillAmount = normalizedHealth;
            healthText.text = $"{newHealth}/{maxHealth}";

            currentHealth = newHealth;
        }

        private IEnumerator HurtPresentation(int damageAmount, int newHealth)
        {
            int maxH = enemy != null ? enemy.MaxHealth : testMaxHealth;

            float damageRatio = maxH == 0 ? 0 : (float)damageAmount / maxH;
            float normalizedHealth = maxH == 0 ? 0 : (float)newHealth / maxH;

            float duration = CheckHeavyHurt(damageRatio, newHealth) ? heavyHurtDuration : normalHurtDuration;
            float textOffsetDuration = CheckHeavyHurt(damageRatio, newHealth) ? heavyHurtTextOffsetDuration : normalHurtTextOffsetDuration;
            float totalHealthDuration = duration + textOffsetDuration;

            float followThroughDuration = CheckHeavyHurt(damageRatio, newHealth) ? heavyFollowThroughDuration : normalFollowThroughDuration;

            Vector3 bodyShake = CheckHeavyHurt(damageRatio, newHealth) ? heavyHurtShakeAmount : normalHurShakeAmount;

            Vector3 hpBarShake = CheckHeavyHurt(damageRatio, newHealth) ? heavyHealthBarShakeAmount : normalHealthBarShakeAmount;
            Vector3 actionShake = CheckHeavyHurt(damageRatio, newHealth) ? heavyActionIconShakeAmount : normalActionIconShakeAmount;
            Vector3 statusShake = CheckHeavyHurt(damageRatio, newHealth) ? heavyStatusIconShakeAmount : normalStatusIconShakeAmount;

            int hurtShakeVibartor = CheckHeavyHurt(damageRatio, newHealth) ? heavyHurtShakeVibartor : normalHurtShakeVibartor;
            float huerShakeRandomNesss = CheckHeavyHurt(damageRatio, newHealth) ? heavyHurtShakeRandomNess : normalHurtShakeRandomNess;
            var hurtShakeRandomnesMode = CheckHeavyHurt(damageRatio, newHealth) ? heavyHurtShakeRandomnessMode : normalHurtShakeRandomnessMode;
            Ease ease = CheckHeavyHurt(damageRatio, newHealth) ? heavyHurtEase : normalHurtEase;

            Sequence healthBarSequence = DOTween.Sequence();
            healthBarSequence.Join(healthBarImage.DOFillAmount(normalizedHealth, totalHealthDuration).SetEase(ease));
            healthBarSequence.Join(DOTween.To(
                () => currentHealth,
                (health) =>
                {
                    currentHealth = health;
                    healthText.text = $"{health}/{maxH}";
                },
                newHealth,
                totalHealthDuration
            ).SetEase(ease));

            Sequence shakeSequence = DOTween.Sequence();

            shakeSequence.Append(whole.transform.DOShakePosition(duration, bodyShake, hurtShakeVibartor, huerShakeRandomNesss, false, false, hurtShakeRandomnesMode).SetEase(ease));

            shakeSequence.Insert(duration, HealthBar.transform.DOShakePosition(followThroughDuration, hpBarShake, hurtShakeVibartor, huerShakeRandomNesss, false, false, hurtShakeRandomnesMode).SetEase(ease));

            foreach (var actionIcon in actionIcons)
            {
                if (actionIcon != null)
                {
                    shakeSequence.Insert(duration, actionIcon.transform.DOShakePosition(followThroughDuration, actionShake, hurtShakeVibartor, huerShakeRandomNesss, false, false, hurtShakeRandomnesMode).SetEase(ease));
                }
            }

            if (battleStatusEffectIconContainer != null)
            {
                foreach (Transform statusIcon in battleStatusEffectIconContainer)
                {
                    shakeSequence.Insert(duration, statusIcon.DOShakePosition(followThroughDuration, statusShake, hurtShakeVibartor, huerShakeRandomNesss, false, false, hurtShakeRandomnesMode).SetEase(ease));
                }
            }

            Sequence finalSequence = DOTween.Sequence();
            finalSequence.Join(healthBarSequence);
            finalSequence.Join(shakeSequence);

            yield return finalSequence.WaitForCompletion();

            DrawHealthBarDirectly(newHealth, maxH);

            bool CheckHeavyHurt(float damageRatio, int newHealth)
            {
                return damageRatio >= heavyHurtRatio || newHealth <= 0;
            }
        }

        private IEnumerator HealPresentation(int newHealth, int currentHealth)
        {
            yield return null;
            DrawHealthBarDirectly(newHealth, enemy != null ? enemy.MaxHealth : testMaxHealth);
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
            nameText.Text = $"{enemy.Data.EnemyName}";

            var healthText = builder.AddMainText(parent);
            healthText.Text = $"{enemy.CurrentHealth}/{enemy.MaxHealth}";

            var availableActionPanel = builder.AddSubPanel(parent);
            availableActionPanel.Header = "행동 종류";
            var toDo = builder.AddNormalText(availableActionPanel.ItemContainer);
            toDo.Text = "ToDo : Implement Enemy Action Description";

            var intendedActionPanel = builder.AddSubPanel(parent);
            intendedActionPanel.Header = "하게 될 행동";
            foreach (var icon in actionIcons)
            {
                icon.OnInspect(builder, intendedActionPanel.ItemContainer);
            }
            
            base.OnInspect(builder, parent);
        }

        [ContextMenu("Test Hurt Presentation")]
        public void TestHurtPresentation()
        {
            DrawHealthBarDirectly(testStartHealth, testMaxHealth);

            int targetHealth = Mathf.Max(0, testStartHealth - testDamage);
            
            StartCoroutine(DelayTestHurtPresentation(testDamage, targetHealth));
        }
        
        private IEnumerator DelayTestHurtPresentation(int testDamage, int targetHealth)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(HurtPresentation(testDamage, targetHealth));
        }

    }
}