using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;

namespace View.BattleView
{
    public abstract class BattleDeckButtonView : ViewBehaviour<IBattleViewEvent>, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [Header("Behaviour")]
        [SerializeField] private RectTransform background;
        [SerializeField] private TextMeshProUGUI deckCountText;
        protected IReadOnlyBattleDeck deck;

        [Header("Presentation")]
        [SerializeField] private float focusingScale;
        [SerializeField] private float focusingPresentationDuration;
        [SerializeField] private Ease focusingPresentationEase;
        private Tween currentFocusingTween;

        public override void OnInitialized()
        {
            eventBus.Subscribe<InitialDeckSettled>(OnInitialDeckSettled);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialDeckSettled>(OnInitialDeckSettled);
        }

        public abstract void OnInitialDeckSettled(InitialDeckSettled payload);

        protected void DrawDeckCountText(int currentCount)
        {
            deckCountText.text = currentCount.ToString();
        }

        protected IEnumerator DrawDeckCountTextPresentation(int currentCount)
        {
            yield return null;
            DrawDeckCountText(currentCount);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            currentFocusingTween?.Kill();
            currentFocusingTween = background.DOScale(focusingScale, CalculateFocusingDuration(transform.localScale.x)).SetEase(focusingPresentationEase);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            currentFocusingTween?.Kill();
            currentFocusingTween = background.DOScale(1, CalculateFocusingDuration(transform.localScale.x)).SetEase(focusingPresentationEase);
        }

        private float CalculateFocusingDuration(float currentScale)
        {
            float originalDelta = Mathf.Abs(focusingScale - 1);
            float currentDelta = Mathf.Abs(focusingScale - currentScale);

            float ratio = originalDelta == 0 ? 0 : currentDelta/originalDelta;

            return focusingPresentationDuration * ratio;
        }

        public abstract void OnPointerClick(PointerEventData eventData);
    }
}
