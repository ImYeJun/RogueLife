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
        [Tooltip("Settings for battle health hurt. (Shake Effect)")]
        [SerializeField] private float battleHealthHurtDuration;
        [SerializeField] private float battleHealthTextOffsetDuration;
        [SerializeField] private Vector3 battleHealthShakeAmount; 
        [SerializeField] private int battleHealthShakeVibrato = 10;
        [SerializeField] private float battleHealthShakeRandomness = 90;
        [SerializeField] private ShakeRandomnessMode battleHealthShakeRandomnessMode = ShakeRandomnessMode.Full;
        [SerializeField] private Ease battleHealthHurtEase;

        [Header("Mentality Hurt Presentation Settings")]
        [Tooltip("Settings for direct hits to Mentality. (Stronger Shake Effect)")]
        [SerializeField] private float mentalityHurtDuration;
        [SerializeField] private float mentalityHurtTextOffsetDuration;
        [SerializeField] private Vector3 mentalityShakeAmount; 
        [SerializeField] private int mentalityShakeVibrato = 15; 
        [SerializeField] private float mentalityShakeRandomness = 90;
        [SerializeField] private ShakeRandomnessMode mentalityShakeRandomnessMode = ShakeRandomnessMode.Full;
        [SerializeField] private Ease mentalityHurtEase;

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

        [Header("Follow-Through Settings (All Shaking)")]
        [Tooltip("Lingering shake duration for child components (e.g., UI, icons) after the main body stops shaking.")]
        [SerializeField] private float battleHealthFollowThroughDuration;
        [SerializeField] private float mentalityFollowThroughDuration;
        [SerializeField] private float overflowedFollowThroughDuration;

        [Space(5)]
        [Tooltip("Common shake settings for all follow-through components.")]
        [SerializeField] private int followThroughShakeVibrato = 15;
        [SerializeField] private float followThroughShakeRandomness = 90;
        [SerializeField] private ShakeRandomnessMode followThroughShakeRandomnessMode = ShakeRandomnessMode.Full;

        [Space(10)]
        [Tooltip("Follow-through shake amounts for Battle Health hurt.")]
        [SerializeField] private Vector3 battleHealthBarShakeAmount; 
        [SerializeField] private Vector3 battleHealthStatusIconShakeAmount; 

        [Space(10)]
        [Tooltip("Follow-through shake amounts for Mentality hurt.")]
        [SerializeField] private Vector3 mentalityHealthBarShakeAmount; 
        [SerializeField] private Vector3 mentalityStatusIconShakeAmount; 

        [Space(10)]
        [Tooltip("Follow-through shake amounts for Overflowed hurt.")]
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
                    sequence.Append(PlayMentalityHurtPresentation(payload, existingBattleHealth, existingMentality, statusEffectIcons));
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

            Sequence result = DOTween.Sequence();

            Sequence healthBarSequence = CreateBarUpdateSequence(
                targetBar: battleHealthBar,
                targetText: battleHealthText,
                startValue: existingBattleHealth,
                endValue: payload.CurrentBattleHealth,
                maxValue: maxHealth,
                barDuration: battleHealthHurtDuration,
                textDuration: battleHealthHurtDuration + battleHealthTextOffsetDuration,
                ease: battleHealthHurtEase
            );

            Sequence actionSequence = DOTween.Sequence();

            actionSequence.Join(whole.DOShakePosition(battleHealthHurtDuration, battleHealthShakeAmount, battleHealthShakeVibrato, battleHealthShakeRandomness, false, false, battleHealthShakeRandomnessMode).SetEase(battleHealthHurtEase));
            actionSequence.Insert(battleHealthHurtDuration, healthBarContainer.DOShakePosition(battleHealthFollowThroughDuration, battleHealthBarShakeAmount, followThroughShakeVibrato, followThroughShakeRandomness, false, false, followThroughShakeRandomnessMode).SetEase(battleHealthHurtEase));
            
            foreach (var icon in statusEffectIcons)
            {
                if (icon == null) continue;
                actionSequence.Insert(battleHealthHurtDuration, icon.DOShakePosition(battleHealthFollowThroughDuration, battleHealthStatusIconShakeAmount, followThroughShakeVibrato, followThroughShakeRandomness, false, false, followThroughShakeRandomnessMode).SetEase(battleHealthHurtEase));
            }

            result.Join(healthBarSequence);
            result.Join(actionSequence);

            return result;
        }

        private Tween PlayMentalityHurtPresentation(PlayerHurt payload, int existingBattleHealth, int existingMentality, List<Transform> statusEffectIcons)
        {
            int maxHealth = payload.Player.Health.MaxMentality;

            Sequence result = DOTween.Sequence();

            Sequence healthBarSequence = CreateBarUpdateSequence(
                targetBar: mentalityBar,
                targetText: mentalityText,
                startValue: existingMentality,
                endValue: payload.CurrentMentality,
                maxValue: maxHealth,
                barDuration: mentalityHurtDuration,
                textDuration: mentalityHurtDuration + mentalityHurtTextOffsetDuration,
                ease: mentalityHurtEase
            );
            
            Sequence actionSequence = DOTween.Sequence();

            actionSequence.Join(whole.DOShakePosition(mentalityHurtDuration, mentalityShakeAmount, mentalityShakeVibrato, mentalityShakeRandomness, false, false, mentalityShakeRandomnessMode).SetEase(mentalityHurtEase));
            actionSequence.Insert(mentalityHurtDuration, healthBarContainer.DOShakePosition(mentalityFollowThroughDuration, mentalityHealthBarShakeAmount, followThroughShakeVibrato, followThroughShakeRandomness, false, false, followThroughShakeRandomnessMode).SetEase(mentalityHurtEase));
            
            foreach (var icon in statusEffectIcons)
            {
                if (icon == null) continue;
                actionSequence.Insert(mentalityHurtDuration, icon.DOShakePosition(mentalityFollowThroughDuration, mentalityStatusIconShakeAmount, followThroughShakeVibrato, followThroughShakeRandomness, false, false, followThroughShakeRandomnessMode).SetEase(mentalityHurtEase));
            }

            result.Join(healthBarSequence);
            result.Join(actionSequence);

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