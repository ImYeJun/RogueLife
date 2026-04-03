using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ViewEvent.BattleView;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;
using UnityEngine.Serialization;

namespace View.BattleView
{
    public class BattleStatusEffectIcon : MonoBehaviour, IInspectable
    {
        private CanvasGroup canvasGroup;
        private LayoutElement layoutElement;

        [SerializeField] private Image effectImage;
        [SerializeField] private TextMeshProUGUI stackText;
        [SerializeField] private TextMeshProUGUI remainTurnText;
        private IReadOnlyBattleStatusEffect currentEffect;
        public IReadOnlyBattleStatusEffect CurrentEffect => currentEffect;

        [SerializeField] float presentationHoldDuration = 0.5f;
        
        [Header("Applied Presentation")]
        [SerializeField] private float appliedPresentationDuration;
        [SerializeField] private Ease appliedPresentationEase;
        
        [Header("Executed Presentation")]
        [SerializeField] private float executedPresentationDuration;
        [SerializeField] private Ease executedPresentationEase;
        [SerializeField] private Vector3 punchAmount;
        [SerializeField] private int punchVibrato;
        [SerializeField] private float punchElasticity;

        [Header("Updated Presentation (Number Count)")]
        [SerializeField] private float timePerCount = 0.1f;
        [SerializeField] private float maxCountDuration = 1.0f;
        [SerializeField] private float updatedPunchDuration;
        [SerializeField] private Ease updatedPunchEase;
        
        [FormerlySerializedAs("updatedPresentationEase")]
        [SerializeField] private Ease updatedCountEase = Ease.Linear; 

        [Header("Removed Presentation")]
        [SerializeField] private float removedPresentationDuration;
        [SerializeField] private Ease removedPresentationEase;

        private int currentDisplayStack;
        private int currentDisplayRemainTurn;

        private void Awake() 
        {
            canvasGroup = GetComponent<CanvasGroup>();
            layoutElement = GetComponent<LayoutElement>();

            canvasGroup.alpha = 0;
            layoutElement.ignoreLayout = true;
        }

        public void Initialize(IReadOnlyBattleStatusEffect effect)
        {
            if (effect == null)
            {
                throw new ArgumentNullException("[BattleStatusEffectIcon/Initialize] The given status effect is null.");
            }

            currentEffect = effect;
            effectImage.sprite = effect.Data.Icon;

            currentDisplayStack = effect.StackCount;
            currentDisplayRemainTurn = effect.RemainTurn;

            ApplyTextUI(currentDisplayRemainTurn, currentDisplayStack);
        }

        public void UpdateState(int currentStack, int remainTurn)
        {
            currentDisplayStack = currentStack;
            currentDisplayRemainTurn = remainTurn;
            ApplyTextUI(currentDisplayRemainTurn, currentDisplayStack);
        }

        private void ApplyTextUI(int remainTurn, int stack)
        {
            stackText.text = stack > 1 ? $"x{stack}" : "";
            remainTurnText.text = currentEffect.IsDurationEternal ? "" : remainTurn.ToString();
        }

        public void OnInspect(IInspectorBuilder builder, RectTransform parent)
        {
            var linkedGroup = builder.AddLinkedGroup(parent);

            var mainText = builder.AddBodyText(linkedGroup.RectTransform);
            string typeColorHex = currentEffect.Data.Type switch
            {
                BattleStatusEffectType.BUFF => "#44FF44",   
                BattleStatusEffectType.DEBUFF => "#FF4444", 
                _ => "#FFFFFF"                              
            };

            var mainContext = $"<color={typeColorHex}>{currentEffect.Data.Name}</color> <size=80%>스택 : {currentEffect.StackCount}";
            if (currentEffect.IsDurationEternal)
            {
                mainContext += "</size>";
            }
            else
            {
                mainContext += $", 남은 턴 : {currentEffect.RemainTurn}</size>";
            }
            mainText.Text = mainContext;

            var captionText = builder.AddCaptionText(linkedGroup.RectTransform);
            captionText.Text = $"{currentEffect.Data.Description}";
        }

        public IEnumerator PlayAppliedPresentation()
        {
            if (this == null || canvasGroup == null || layoutElement == null) yield break;

            layoutElement.ignoreLayout = false;
            canvasGroup.alpha = 0;
            
            yield return canvasGroup.DOFade(1, appliedPresentationDuration)
                .SetEase(appliedPresentationEase)
                .SetLink(gameObject)
                .WaitForCompletion();

            if (this == null) yield break;
            yield return new WaitForSeconds(presentationHoldDuration);
        }

        public IEnumerator PlayExecutedPresentation()
        {
            if (this == null) yield break;

            yield return transform.DOPunchScale(punchAmount, executedPresentationDuration, punchVibrato, punchElasticity)
                .SetEase(executedPresentationEase)
                .SetLink(gameObject)
                .WaitForCompletion();

            if (this == null) yield break;
            yield return new WaitForSeconds(presentationHoldDuration);
        }

        public IEnumerator PlayUpdatedPresentation(int targetStack, int targetRemainTurn)
        {
            if (this == null) yield break;

            Sequence seq = DOTween.Sequence();
            seq.SetLink(gameObject);

            seq.Append(transform.DOPunchScale(punchAmount, updatedPunchDuration, punchVibrato, punchElasticity).SetEase(updatedPunchEase));

            int stackDiff = Mathf.Abs(targetStack - currentDisplayStack);
            int turnDiff = Mathf.Abs(targetRemainTurn - currentDisplayRemainTurn);
            
            bool isFirstCountTween = true;

            if (stackDiff > 0)
            {
                float duration = Mathf.Min(stackDiff * timePerCount, maxCountDuration);
                
                Tween stackTween = DOVirtual.Int(currentDisplayStack, targetStack, duration, (val) =>
                {
                    if (this == null) return;
                    currentDisplayStack = val;
                    ApplyTextUI(currentDisplayRemainTurn, currentDisplayStack);
                }).SetEase(updatedCountEase);

                seq.Append(stackTween);
                isFirstCountTween = false;
            }

            if (turnDiff > 0)
            {
                float duration = Mathf.Min(turnDiff * timePerCount, maxCountDuration);
                
                Tween turnTween = DOVirtual.Int(currentDisplayRemainTurn, targetRemainTurn, duration, (val) =>
                {
                    if (this == null) return;
                    currentDisplayRemainTurn = val;
                    ApplyTextUI(currentDisplayRemainTurn, currentDisplayStack);
                }).SetEase(updatedCountEase);

                if (isFirstCountTween)
                {
                    seq.Append(turnTween);
                }
                else
                {
                    seq.Join(turnTween);
                }
            }

            if (seq.Duration() > 0)
            {
                yield return seq.WaitForCompletion();
                
                if (this == null) yield break;
                yield return new WaitForSeconds(presentationHoldDuration);
            }
        }

        public IEnumerator PlayRemovedPresentation()
        {
            if (this == null || canvasGroup == null) yield break;

            canvasGroup.alpha = 1;
            
            yield return canvasGroup.DOFade(0, removedPresentationDuration)
                .SetEase(removedPresentationEase)
                .SetLink(gameObject)
                .WaitForCompletion();

            if (this == null) yield break;
            yield return new WaitForSeconds(presentationHoldDuration);
        }

#if UNITY_EDITOR
        [ContextMenu("Play Applied Presentation")]
        public void TestAppliedPresentation()
        {
            canvasGroup.alpha = 0;
            StartCoroutine(DelayTestPlay(PlayAppliedPresentation()));
            canvasGroup.alpha = 1;
        }

        [ContextMenu("Play Executed Presentation")]
        public void TestExecutedPresentation()
        {
            StartCoroutine(DelayTestPlay(PlayExecutedPresentation()));
        }

        [ContextMenu("Play Updated Presentation (Test Count +5)")]
        public void TestUpdatedPresentation()
        {
            StartCoroutine(DelayTestPlay(PlayUpdatedPresentation(currentDisplayStack + 5, currentDisplayRemainTurn)));
        }

        [ContextMenu("Play Removed Presentation")]
        public void TestRemovedPresentation()
        {
            canvasGroup.alpha = 1;
            StartCoroutine(DelayTestPlay(PlayRemovedPresentation()));
            canvasGroup.alpha = 0;
        }

        private IEnumerator DelayTestPlay(IEnumerator presentation)
        {
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(presentation);
        }
#endif
    }
}