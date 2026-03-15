using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewEvent.BattleView;

namespace View.BattleView
{
    public class PlayerHurtPresentation : MonoBehaviour
    {
        private Transform whole;
        private Transform healthBarContainer;
        private Image mentalityBar;
        private TextMeshProUGUI mentalityText;
        private Image battleHealthBar;
        private TextMeshProUGUI battleHealthText;

        private bool isFirstOverflowHurt = true;

        [Header("Battle Health Hurt Presentation Settings")]
        [SerializeField, Range(0, 1f)] private float heavyHurtRatio;
        
        [Space(5)]
        [Tooltip("Settings for normal battle health hurt.")]
        [SerializeField] private float normalHurtDuration;
        [SerializeField] private float normalHurtTextOffsetDuration;
        [SerializeField] private Vector3 normalHurtShakeAmount;
        [SerializeField] private int normalHurtShakeVibrato = 10;
        [SerializeField] private float normalHurtShakeRandomness = 90;
        [SerializeField] private ShakeRandomnessMode normalHurtShakeRandomnessMode = ShakeRandomnessMode.Full;
        [SerializeField] private Ease normalHurtEase;

        [Space(5)]
        [Tooltip("Settings for heavy battle health hurt.")]
        [SerializeField] private float heavyHurtDuration;
        [SerializeField] private float heavyHurtTextOffsetDuration;
        [SerializeField] private Vector3 heavyHurtShakeAmount;
        [SerializeField] private int heavyHurtShakeVibrato = 10;
        [SerializeField] private float heavyHurtShakeRandomness = 90;
        [SerializeField] private ShakeRandomnessMode heavyHurtShakeRandomnessMode = ShakeRandomnessMode.Full;
        [SerializeField] private Ease heavyHurtEase;

        [Header("First Mentality Hurt Presentation Settings")]
        [Tooltip("Settings for the first direct hit to Mentality.")]
        [SerializeField] private float firstMentalityHurtDuration;
        [SerializeField] private float firstMentalityHurtTextOffsetDuration;
        [SerializeField] private Vector3 firstMentalityPunchAmount;
        [SerializeField] private int firstMentalityPunchVibrato = 10;
        [SerializeField] private float firstMentalityPunchElasticity = 1f;
        [SerializeField] private Vector3 firstMentalityShakeAmount;
        [SerializeField] private int firstMentalityShakeVibrato = 10;
        [SerializeField] private float firstMentalityShakeRandomness = 90;
        [SerializeField] private ShakeRandomnessMode firstMentalityShakeRandomnessMode = ShakeRandomnessMode.Full;
        [SerializeField] private Ease firstMentalityHurtEase;
        [SerializeField] private Color firstMentalityGlitchColor = Color.magenta;

        [Header("Normal Mentality Hurt Presentation Settings")]
        [Tooltip("Settings for subsequent direct hits to Mentality. Usually slightly shorter and less dramatic than the first hit.")]
        [SerializeField] private float normalMentalityHurtDuration;
        [SerializeField] private float normalMentalityHurtTextOffsetDuration;
        [SerializeField] private Vector3 normalMentalityPunchAmount;
        [SerializeField] private int normalMentalityPunchVibrato = 10;
        [SerializeField] private float normalMentalityPunchElasticity = 1f;
        [SerializeField] private Vector3 normalMentalityShakeAmount;
        [SerializeField] private Ease normalMentalityHurtEase;
        [SerializeField] private Color normalMentalityGlitchColor = Color.red;

        [Header("First Overflowed Mentality Hurt Presentation Settings")]
        [Tooltip("Step 1: Battle health broken duration & shake.")]
        [SerializeField] private float firstOverflow_BattleHealthDuration;
        [SerializeField] private Vector3 firstOverflow_BattleHealthShakeAmount;
        [SerializeField] private int firstOverflow_BattleHealthShakeVibrato = 20;

        [Tooltip("Step 2: Hit Stop presentation when the defense is initially breached.")]
        [Space(2)]
        [SerializeField] private float firstOverflow_HitStopDuration = 0.15f;
        [SerializeField, Range(0f, 1f)] private float firstOverflow_HitStopTimeScale = 0.05f;
        
        [Tooltip("Step 3: Mentality damage presentation")]
        [Space(2)]
        [SerializeField] private float firstOverflow_MentalityDuration;
        [SerializeField] private Vector3 firstOverflow_MentalityPunchAmount;
        [SerializeField] private int firstOverflow_MentalityPunchVibrato = 15;
        [SerializeField] private float firstOverflow_MentalityPunchElasticity = 1f;
        [SerializeField] private Vector3 firstOverflow_MentalityShakeAmount;
        [SerializeField] private Ease firstOverflow_MentalityEase;

        [Header("Normal Overflowed Hurt Presentation Settings")]
        [Tooltip("Settings for when Battle Health is broken again after being recovered. Less Hit Stop, faster transition.")]
        [SerializeField] private float normalOverflow_BattleHealthDuration;
        [SerializeField] private Vector3 normalOverflow_BattleHealthShakeAmount;
        [SerializeField] private float normalOverflow_HitStopDuration = 0.05f;
        [SerializeField, Range(0f, 1f)] private float normalOverflow_HitStopTimeScale = 0.2f;
        [SerializeField] private Color normalOverflow_FlashColor = Color.gray;
        [SerializeField] private float normalOverflow_MentalityDuration;
        [SerializeField] private Vector3 normalOverflow_MentalityPunchAmount;
        [SerializeField] private Vector3 normalOverflow_MentalityShakeAmount;
        [SerializeField] private Ease normalOverflow_MentalityEase;

        [Header("Follow-Through Settings")]
        [Tooltip("Lingering shake duration for child components (e.g., UI, icons) after the main body stops shaking.")]
        [SerializeField] private float normalFollowThroughDuration;
        [SerializeField] private float heavyFollowThroughDuration;
        [SerializeField] private float mentalityFollowThroughDuration;
        [SerializeField] private float overflowedFollowThroughDuration;

        [Space(10)]
        [Tooltip("Follow-through for Battle Health hurt.")]
        [SerializeField] private Vector3 normalHealthBarShakeAmount;
        [SerializeField] private Vector3 heavyHealthBarShakeAmount;
        [SerializeField] private Vector3 normalStatusIconShakeAmount;
        [SerializeField] private Vector3 heavyStatusIconShakeAmount;

        [Space(10)]
        [Tooltip("Follow-through for Mentality hurt.")]
        [SerializeField] private Vector3 mentalityHealthBarPunchAmount;
        [SerializeField] private Vector3 mentalityStatusIconPunchAmount;

        [Space(10)]
        [Tooltip("Follow-through for Overflowed hurt.")]
        [SerializeField] private Vector3 overflowedHealthBarShakeAmount;
        [SerializeField] private Vector3 overflowedStatusIconShakeAmount;
        public void Initialize(
            Transform whole,
            Transform healthBar, Image mentalityBar, TextMeshProUGUI mentalityText, Image battleHealthBar, TextMeshProUGUI battleHealthText)
        {
            this.whole = whole;
            this.healthBarContainer = healthBar; 
            this.mentalityBar = mentalityBar;    
            this.mentalityText = mentalityText;
            this.battleHealthBar = battleHealthBar;
            this.battleHealthText = battleHealthText;

            isFirstOverflowHurt = true;
        }

        public IEnumerator Play(PlayerHurt payload, int existingBattleHealth, int existingMentality, List<Transform> statusEffectIcons)
        {
            var sequence = DOTween.Sequence();
            if (payload.IsOverflowed)
            {
                sequence.Append(PlayOverflowedHurtPresentation(payload, existingBattleHealth, existingMentality, statusEffectIcons));
            }
            else
            {
                if (payload.MentalityDamage > 0)
                {
                    sequence.Append(PlayNormalMentalityHurtPresentation(payload, existingBattleHealth, existingMentality, statusEffectIcons));
                }
                if (payload.BattleHealthDamage > 0)
                {
                    sequence.Append(PlayBattleHealthHurtPresentation(payload, existingBattleHealth, existingMentality, statusEffectIcons));
                }
            }

            return WrapAsCoroutine(sequence);
        }

        private Tween PlayBattleHealthHurtPresentation(PlayerHurt payload, int existingBattleHealth, int existingMentality, List<Transform> statusEffectIcons)
        {
            int maxHealth = payload.Player.Health.MaxBattleHealth;

            float damageRatio = (float)payload.BattleHealthDamage/existingBattleHealth;
            float normalizedHealth = (float)payload.CurrentBattleHealth/maxHealth;

            float duration = IsHeavyHurt(damageRatio) ? heavyHurtDuration : normalHurtDuration;
            float textOffsetDuration = IsHeavyHurt(damageRatio) ? heavyHurtTextOffsetDuration : normalHurtTextOffsetDuration;
            Vector3 shakeAmount = IsHeavyHurt(damageRatio) ? heavyHurtShakeAmount : normalHurtShakeAmount;
            int shakeVibrato = IsHeavyHurt(damageRatio) ? heavyHurtShakeVibrato : normalHurtShakeVibrato;
            float shakeRandomness = IsHeavyHurt(damageRatio) ? heavyHurtShakeRandomness : normalHurtShakeRandomness;
            ShakeRandomnessMode shakeRandomnesMode = IsHeavyHurt(damageRatio) ? heavyHurtShakeRandomnessMode : normalHurtShakeRandomnessMode;
            Ease ease = IsHeavyHurt(damageRatio) ? heavyHurtEase : normalHurtEase;
            float followThroughDuration = IsHeavyHurt(damageRatio) ? heavyFollowThroughDuration : normalFollowThroughDuration;
            Vector3 healthBarShakeAmount = IsHeavyHurt(damageRatio) ? heavyHealthBarShakeAmount : normalHealthBarShakeAmount;
            Vector3 statusIconShakeAmount = IsHeavyHurt(damageRatio) ? heavyStatusIconShakeAmount : normalStatusIconShakeAmount;
            
            Sequence result = DOTween.Sequence();

            Sequence healthBarSequence = CreateBarUpdateSequence(
                targetBar : battleHealthBar,
                targetText : battleHealthText,
                startValue : existingBattleHealth,
                endValue : payload.CurrentBattleHealth,
                maxValue : maxHealth,
                barDuration : duration,
                textDuration : duration + textOffsetDuration,
                ease : ease
            );

            Sequence shakeSeqeuence = DOTween.Sequence();
            shakeSeqeuence.Join(whole.DOShakePosition(duration, shakeAmount, shakeVibrato, shakeRandomness, false, false, shakeRandomnesMode)).SetEase(ease);
            shakeSeqeuence.Insert(duration, healthBarContainer.DOShakePosition(followThroughDuration, healthBarShakeAmount, shakeVibrato, shakeRandomness, false, false, shakeRandomnesMode)).SetEase(ease);
            foreach (var icon in statusEffectIcons)
            {
                shakeSeqeuence.Insert(duration, icon.DOShakePosition(followThroughDuration, statusIconShakeAmount, shakeVibrato, shakeRandomness, false, false, shakeRandomnesMode)).SetEase(ease);
            }

            result.Join(healthBarSequence);
            result.Join(shakeSeqeuence);

            return result;

            bool IsHeavyHurt(float damageRatio)
            {
                return damageRatio >= heavyHurtRatio;
            }
        }

        private Tween PlayNormalMentalityHurtPresentation(PlayerHurt payload, int existingBattleHealth, int existingMentality, List<Transform> statusEffectIcons)
        {
            int maxHealth = payload.Player.Health.MaxMentality;

            Sequence result = DOTween.Sequence();

            Sequence healthBarSequence = CreateBarUpdateSequence(
                targetBar : mentalityBar,
                targetText : mentalityText,
                startValue : existingMentality,
                endValue : payload.CurrentMentality,
                maxValue : maxHealth,
                barDuration : normalMentalityHurtDuration,
                textDuration : normalMentalityHurtDuration + normalMentalityHurtTextOffsetDuration,
                ease : normalMentalityHurtEase
            );
            
            Sequence punchSequence = DOTween.Sequence();
            punchSequence.Append(whole.DOPunchPosition(normalMentalityPunchAmount, normalMentalityHurtDuration, normalMentalityPunchVibrato, normalMentalityPunchElasticity)).SetEase(normalMentalityHurtEase);
            punchSequence.Insert(normalMentalityHurtDuration, healthBarContainer.DOPunchPosition(mentalityHealthBarPunchAmount, mentalityFollowThroughDuration)).SetEase(normalMentalityHurtEase);
            foreach (var icon in statusEffectIcons)
            {
                punchSequence.Insert(normalMentalityHurtDuration, icon.DOPunchPosition(mentalityStatusIconPunchAmount, mentalityFollowThroughDuration)).SetEase(normalMentalityHurtEase);
            }

            result.Join(healthBarSequence);
            result.Join(punchSequence);

            return result;
        }

        private Tween PlayOverflowedHurtPresentation(PlayerHurt payload, int existingBattleHealth, int existingMentality, List<Transform> statusEffectIcons)
        {
            throw new NotImplementedException();
        }

        private IEnumerator WrapAsCoroutine(Tween tween)
        {
            yield return tween.WaitForCompletion();
        }

        private Sequence CreateBarUpdateSequence(
            Image targetBar, 
            TextMeshProUGUI targetText, 
            int startValue, 
            int endValue, 
            int maxValue, 
            float barDuration, 
            float textDuration, 
            Ease ease)
        {
            float targetNormalized = maxValue == 0 ? 0 : (float)endValue / maxValue;
            
            Sequence sequence = DOTween.Sequence();

            sequence.Join(targetBar.DOFillAmount(targetNormalized, barDuration).SetEase(ease));
            sequence.Join(DOTween.To(
                () => startValue,
                (val) =>
                {
                    targetText.text = $"{val}/{maxValue}";
                },
                endValue,
                textDuration
            ).SetEase(ease));

            return sequence;
        }
#if UNITY_EDITOR
        [Header("Test Only")]
        [SerializeField] private int testMaxBattleHealth = 100;
        [SerializeField] private int testStartBattleHealth = 100;
        [SerializeField] private int testBattleHealthDamage = 30;

        [Space(5)]
        [SerializeField] private int testMaxMentality = 100;
        [SerializeField] private int testStartMentality = 100;
        [SerializeField] private int testMentalityDamage = 20;

        [Space(5)]
        [SerializeField] private bool testIsOverflowed = false;
        
        [Space(5)]
        [SerializeField] private List<RectTransform> testStatusEffectIcons = new List<RectTransform>();

        private void DrawHealthBarsDirectly(int currentBh, int maxBh, int currentMen, int maxMen)
        {
            if (battleHealthBar != null)
            {
                battleHealthBar.fillAmount = maxBh == 0 ? 0 : (float)currentBh / maxBh;
            }
            if (battleHealthText != null)
            {
                battleHealthText.text = $"{currentBh}/{maxBh}";
            }

            if (mentalityBar != null)
            {
                mentalityBar.fillAmount = maxMen == 0 ? 0 : (float)currentMen / maxMen;
            }
            if (mentalityText != null)
            {
                mentalityText.text = $"{currentMen}/{maxMen}";
            }
        }

        [ContextMenu("Test Hurt Presentation")]
        public void TestHurtPresentation()
        {
            DrawHealthBarsDirectly(testStartBattleHealth, testMaxBattleHealth, testStartMentality, testMaxMentality);
            StartCoroutine(DelayTestHurtPresentation());
        }

        private IEnumerator DelayTestHurtPresentation()
        {
            yield return new WaitForSeconds(0.5f);

            int targetBattleHealth = Mathf.Max(0, testStartBattleHealth - testBattleHealthDamage);
            int targetMentality = Mathf.Max(0, testStartMentality - testMentalityDamage);

            MockBattlePlayer mockPlayer = new MockBattlePlayer(
                startBh: testStartBattleHealth, 
                maxBh: testMaxBattleHealth, 
                startMen: testStartMentality, 
                maxMen: testMaxMentality
            );

            PlayerHurt dummyPayload = new PlayerHurt(
                sequenceId: 0, 
                player: mockPlayer,
                battleHealthDamage: testBattleHealthDamage, 
                mentalityDamage: testMentalityDamage, 
                currentBattleHealth: targetBattleHealth, 
                currentMentality: targetMentality, 
                isOverflowed: testIsOverflowed
            );
            
            yield return StartCoroutine(Play(dummyPayload, testStartBattleHealth, testStartMentality, testStatusEffectIcons.Select(icon => (Transform)icon).ToList()));
        }
#endif

#if UNITY_EDITOR
    public class MockHealth : IReadOnlyHealth
    {
        public int CurrentBattleHealth { get; set; }
        public int CurrentMentality { get; set; }
        public int MaxBattleHealth { get; set; }
        public int MaxMentality { get; set; }
        
        public float NormalizedBattleHealth => MaxBattleHealth == 0 ? 0 : (float)CurrentBattleHealth / MaxBattleHealth;
        public float NomarlizedMentality => MaxMentality == 0 ? 0 : (float)CurrentMentality / MaxMentality;
    }

    public class MockBattlePlayer : IReadOnlyBattlePlayer
    {
        private MockHealth mockHealth;
        public IReadOnlyHealth Health => mockHealth;

        public IReadOnlyDictionary<BattleStatusEffectData, BattleStatusEffect> CurrentBuffs => new Dictionary<BattleStatusEffectData, BattleStatusEffect>();
        public IReadOnlyDictionary<BattleStatusEffectData, BattleStatusEffect> CurrentDebuffs => new Dictionary<BattleStatusEffectData, BattleStatusEffect>();

        public MockBattlePlayer(int startBh, int maxBh, int startMen, int maxMen)
        {
            mockHealth = new MockHealth()
            {
                CurrentBattleHealth = startBh,
                MaxBattleHealth = maxBh,
                CurrentMentality = startMen,
                MaxMentality = maxMen
            };
        }

        public List<BattleStatusEffect> GetBattleStatusEffects(BattleStatusEffectType type = BattleStatusEffectType.ANY)
        {
            return new List<BattleStatusEffect>();
        }
    }
#endif
    }
}