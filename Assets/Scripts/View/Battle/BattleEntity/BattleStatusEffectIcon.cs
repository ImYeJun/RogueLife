using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ViewEvent.BattleView;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;
using UnityEditor.Overlays;

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

        [Header("Applied Presentation")]
        [SerializeField] private float appliedPresentationDuration;
        [SerializeField] private Ease appliedPresentationEase;
        [Header("Executed Presentation")]
        [SerializeField] private float executedPresentationDuration;
        [SerializeField] private Ease executedPresentationEase;
        [SerializeField] private Vector3 punchAmount;
        [SerializeField] private int punchVibrato;
        [SerializeField] private float punchElasticity;
        [Header("Applied Presentation")]
        [SerializeField] private float removedPresentationDuration;
        [SerializeField] private Ease removedPresentationEase;

        private void Awake() {
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

            UpdateState(effect.RemainTurn, effect.StackCount);
        }

        public void UpdateState(int remainTurn, int currentStack)
        {
            stackText.text = currentStack > 1 ? $"x{currentStack}" : "";
            remainTurnText.text = currentEffect.IsDurationEternal ? "" : remainTurn.ToString();
        }

        public void OnInspect(IInspectorBuilder builder, RectTransform parent)
        {
            var normalText = builder.AddNormalText(parent);
            normalText.Text = $"({currentEffect.Data.Name}) 스택 : {currentEffect.StackCount}, 남은 턴  : {currentEffect.RemainTurn}";
        }

        public IEnumerator PlayAppliedPresentation()
        {
            layoutElement.ignoreLayout = false;
            canvasGroup.alpha = 0;
            yield return canvasGroup.DOFade(1, appliedPresentationDuration).SetEase(appliedPresentationEase);
        }
        public IEnumerator PlayExectuedPresentation()
        {
            yield return transform.DOPunchScale(punchAmount, executedPresentationDuration, punchVibrato, punchElasticity).SetEase(executedPresentationEase);
        }
        public IEnumerator PlayUpdatedPresentation()
        {
            yield return null; //TODO Implement it
        }
        public IEnumerator PlayRemovedPresentation()
        {
            canvasGroup.alpha = 1;
            yield return canvasGroup.DOFade(0, removedPresentationDuration).SetEase(removedPresentationEase);
        }

#if UNITY_EDITOR
        [ContextMenu("Play Applied Presentation ")]
        public void TestAppliedPresentation()
        {
            canvasGroup.alpha = 0;
            StartCoroutine(DelayTestPlay(PlayAppliedPresentation()));
            canvasGroup.alpha = 1;
        }

        [ContextMenu("Play Exectued Presentation ")]
        public void TestExectuedPresentation()
        {
            StartCoroutine(DelayTestPlay(PlayExectuedPresentation()));
        }

        [ContextMenu("Play Removed Presentation ")]
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