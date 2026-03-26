using UnityEngine;
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
        [SerializeField] private CanvasGroup entityStatusCanvasGroup;
        [SerializeField] private CanvasGroup plannedActionsCanvasGroup;
        [SerializeField, FormerlySerializedAs("healthBar")] private Image healthBarImage;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private GameObject actionIconPrefab;
        [SerializeField] private GameObject inspectActionIconPrefab;
        [SerializeField] private RectTransform actionIconView;
        [SerializeField] private RectTransform actionIconsContainer;

        private List<BattleEnemyActionIcon> actionIcons = new List<BattleEnemyActionIcon>();

        [Header("Action Presentation Setting")]
        [SerializeField] private float actionDuration = 0.5f;

        [Header("Fade Presentation Settings")]
        [SerializeField] protected float fadeDuration = 0.5f;

        [Header("Hurt Presentation Settings")]
        [SerializeField] private AudioData normalHurtSFX;
        [SerializeField] private AudioData heavyHurtSFX;
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

        [Header("Heal Presentation Settings")]
        [Tooltip("Settings for enemy heal presentation.")]
        [SerializeField] private AudioData healSFX;
        [SerializeField] private float healDuration = 0.3f;
        [SerializeField] private float healTextOffsetDuration = 0.2f;
        [SerializeField] private Ease healEase = Ease.OutQuad;

        [Header("Positioning Presentation Setting")]
        [SerializeField] private float positionDuration = 0.4f;
        [SerializeField] private Ease positioningEase = Ease.OutCubic;

        [Header("Test Only")]
        [SerializeField] private int testMaxHealth = 100;
        [SerializeField] private int testStartHealth = 100;
        [SerializeField] private int testDamage = 30;
        [SerializeField] private int testHealAmount = 25;

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
            eventBus.Subscribe<EnemyActionExecuted>(OnEnemyActionExecuted);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            eventBus?.Unsubscribe<EnemyActionPlanned>(OnEnemyActionPlanned);
            eventBus?.Unsubscribe<EnemyTurnEnded>(OnEnemyTurnEndend);
            eventBus?.Unsubscribe<EnemyHurt>(OnEnemyHurt);
            eventBus?.Unsubscribe<EnemyHealed>(OnEnemyHealed);
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

        public void SetInvisibleDirectly()
        {
            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = 0f;
                spriteRenderer.color = c;
            }
            if (entityStatusCanvasGroup != null)
            {
                entityStatusCanvasGroup.alpha = 0f;
            }
            if (plannedActionsCanvasGroup != null)
            {
                plannedActionsCanvasGroup.alpha = 0f;
            }
        }

        public Tween PlayAppearPresentation()
        {
            Sequence seq = DOTween.Sequence();

            if (spriteRenderer != null)
            {
                seq.Join(spriteRenderer.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad));
            }
            if (entityStatusCanvasGroup != null)
            {
                seq.Join(entityStatusCanvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad));
            }
            if (plannedActionsCanvasGroup != null)
            {
                seq.Join(plannedActionsCanvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad));
            }

            return seq;
        }

        public Tween PlayDisappearPresentation()
        {
            Sequence seq = DOTween.Sequence();

            if (spriteRenderer != null)
            {
                seq.Join(spriteRenderer.DOFade(0f, fadeDuration).SetEase(Ease.InQuad));
            }
            if (entityStatusCanvasGroup != null)
            {
                seq.Join(entityStatusCanvasGroup.DOFade(0f, fadeDuration).SetEase(Ease.InQuad));
            }
            if (plannedActionsCanvasGroup != null)
            {
                seq.Join(plannedActionsCanvasGroup.DOFade(0f, fadeDuration).SetEase(Ease.InQuad));
            }

            return seq;
        }

        public Tween UpdatePosition(Vector2 targetPosition, int positionIndex)
        {
            bodyView.SetSpriteSortingOrder(positionIndex);
            return transform.DOMove(targetPosition, positionDuration).SetEase(positioningEase);
        }

        public void OnEnemyActionPlanned(EnemyActionPlanned payload)
        {
            if (enemy == null)
            {
                throw new InvalidOperationException("[BattleEnemyView/OnEnemyActionPlanned] The enemy entity is not initialized yet.");
            }

            if (!payload.Enemy.Equals(enemy)) { return; }
            actionIcons.Clear();

            int actionCount = payload.PlannedActions.Count;
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyActionPlanned_BaseIconAction, PlayActionPlannedPresentation(actionCount),
                () =>
                {
                    actionIconsContainer.sizeDelta = new Vector2(actionCount, ICONS_CONAINTER_HEIGHT);
                } );
            
            for (int i = 0; i < actionCount; i++)
            {
                var action = payload.PlannedActions[i];
                var iconObject = Instantiate(actionIconPrefab, actionIconsContainer);
                var actionIcon = iconObject.GetComponent<BattleEnemyActionIcon>();
                actionIcon.Initialize(action);
                actionIcons.Add(actionIcon);
                actionIcon.SetUnshown();

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

            var actionIconView = actionIcons.FirstOrDefault(view => view.Action == payload.Action && !view.HasExecuted);
            //* Notice that enemy action reference is not cloned but same reference.

            if (actionIconView is null)
            {
                throw new InvalidOperationException("[BattleEnemyView/OnEnemyActionExecuted] The Enemy doesn't have given action.");
            }

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyActionExecuted_ActorAction, PlayActionPresentation(actionIconView));
            actionIconView.HasExecuted = true;
        }
        private IEnumerator PlayActionPresentation(BattleEnemyActionIcon actionIcon)
        {
            bodyView.SetActionSprite();
            StartCoroutine(actionIcon.PlayExecutedPresentation());
            yield return new WaitForSeconds(actionDuration);
            bodyView.SetIdleSprite();
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
                throw new InvalidOperationException("[BattleEnemyView/OnEnemyHurt] The enemy entity is not initialized yet.");
            }

            if (!payload.Enemy.Equals(enemy)) { return; }

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyHurt_HealthBarPresentation, HurtPresentation(payload.Damage, payload.CurrentHealth));
        }

        private void OnEnemyHealed(EnemyHealed payload)
        {
            if (enemy == null)
            {
                throw new InvalidOperationException("[BattleEnemyView/OnEnemyHealed] The enemy entity is not initialized yet.");
            }

            if (!payload.Enemy.Equals(enemy)) { return; }

            int startHealth = currentHealth;
            int targetHealth = payload.CurrentHealth;

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.EnemyHeal_HealthBarPresentation, HealPresentation(startHealth, targetHealth), 
                () => 
                {
                    DrawHealthBarDirectly(targetHealth, enemy.MaxHealth);
                }
            );

            currentHealth = targetHealth;
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

            AudioData audioData = CheckHeavyHurt(damageRatio, newHealth) ? heavyHurtSFX : normalHurtSFX;

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

            SoundManager.Instance?.PlaySoundEffectWithRandomPitch(audioData);
            yield return finalSequence.WaitForCompletion();

            DrawHealthBarDirectly(newHealth, maxH);

            bool CheckHeavyHurt(float damageRatio, int newHealth)
            {
                return damageRatio >= heavyHurtRatio || newHealth <= 0;
            }
        }

        private IEnumerator HealPresentation(int startHealth, int targetHealth)
        {
            int maxH = enemy != null ? enemy.MaxHealth : testMaxHealth;
            float targetNormalized = maxH == 0 ? 0 : (float)targetHealth / maxH;
            float totalDuration = healDuration + healTextOffsetDuration;

            Sequence sequence = DOTween.Sequence();

            sequence.Join(healthBarImage.DOFillAmount(targetNormalized, healDuration).SetEase(healEase));

            int tempHealth = startHealth;
            sequence.Join(DOTween.To(
                () => tempHealth,
                (val) =>
                {
                    tempHealth = val;
                    healthText.text = $"{tempHealth}/{maxH}";
                },
                targetHealth,
                totalDuration
            ).SetEase(healEase));

            SoundManager.Instance?.PlaySoundEffectWithRandomPitch(healSFX);
            yield return sequence.WaitForCompletion();
        }


        public override void OnInspect(IInspectorBuilder builder, RectTransform parent)
        {
            var nameText = builder.AddNameText(parent);
            nameText.Text = $"{enemy.Data.EnemyName}";

            var healthText = builder.AddHeader(parent);
            healthText.Text = $"{enemy.CurrentHealth}/{enemy.MaxHealth}";

            var availableActionPanel = builder.AddSubPanel(parent);
            availableActionPanel.Header = "행동 종류";
            
            foreach (var behaviour in enemy.Data.BehaviourDescriptions)
            {
                var actionInfoPanelObject = builder.AddEnemyActionInfoPanel(availableActionPanel.ItemContainer);
                var actionInfoPanel = actionInfoPanelObject.GetComponent<InspectorEnemyActionInfoPanel>();
                
                var action = enemy.AvailableActions[behaviour.Id];
                
                List<string> statusEffectTexts = new List<string>();
                foreach (var associatedStatusEffect in behaviour.AssociatedStatusEffectIds)
                {
                    var data = commander.GetStatusEffectData(associatedStatusEffect);
                    statusEffectTexts.Add($"{data.Name} : {data.Description}");
                }

                actionInfoPanel.Initialize(action, behaviour.Description, statusEffectTexts, builder);
            }

            var intendedActionPanel = builder.AddSubPanel(parent);
            intendedActionPanel.Header = "하게 될 행동";

            var intendedActionhorizontalLayoutGroup = builder.AddHorizontalLayout(intendedActionPanel.ItemContainer);
            for (int i = 0; i < actionIcons.Count; i++)
            {
                var actionIconObject = Instantiate(inspectActionIconPrefab, intendedActionhorizontalLayoutGroup.transform);
                actionIconObject.transform.localScale = Vector3.one;
                var actionIcon = actionIconObject.GetComponent<BattleEnemyActionIcon>();
                actionIcon.Initialize(actionIcons[i].Action);
            }
            intendedActionhorizontalLayoutGroup.LayoutGroup.spacing = 10;
            
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
            yield return StartCoroutine(HurtPresentation(testDamage, targetHealth));
        }

        [ContextMenu("Test Heal Presentation")]
        public void TestHealPresentation()
        {
            DrawHealthBarDirectly(testStartHealth, testMaxHealth);

            int targetHealth = Mathf.Min(testMaxHealth, testStartHealth + testHealAmount);
            
            StartCoroutine(DelayTestHealPresentation(testStartHealth, targetHealth));
        }

        private IEnumerator DelayTestHealPresentation(int startHealth, int targetHealth)
        {
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(HealPresentation(startHealth, targetHealth));
        }
    }
}