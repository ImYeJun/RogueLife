using System;
using System.Collections;
using DG.Tweening; 
using UnityEngine;
using UnityEngine.EventSystems;
using View.Core;
using ViewEvent.BattleView;

namespace View.BattleView
{
    public class BattleCardView : ViewBehaviour<IBattleViewEvent>, IPointerClickHandler
    {
        [Header("Settings")]
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float popUpDistance = 0.5f; 
        [SerializeField] private SharedCardView sharedCardView;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Focus Presentation")]
        [SerializeField] private float focusMoveDuration = 0.2f;
        [SerializeField] private Ease focusMoveEase = Ease.OutBack;

        [Header("Process Card Presentation")]
        [SerializeField] private float triggerFadeDuration = 0.5f;
        [SerializeField] private Ease triggerFadeEase = Ease.Linear;
        [SerializeField] private float processMoveDuration = 0.5f;
        [SerializeField] private float processRotateDuration = 0.5f;
        [SerializeField] private Ease processMoveEase = Ease.Linear;
        [SerializeField] private Ease processRotateEase = Ease.Linear;
        [SerializeField] private Ease processScaleEase = Ease.Linear;

        [Header("Resolve Card Presentation")]
        [SerializeField] private float resolveFadeDuration = 0.5f;
        [SerializeField] private Ease resolveFadeEase = Ease.Linear;

        private Action<BattleCardView> onCardClicked; 
        private Vector3 baseLocalPosition;
        private Vector3 baseLocalEulerAngles;
        private Vector3 baseLocalScale = Vector3.one;
        private Tween currentTween;

        public Card Card => sharedCardView.Card;

        
        public override void OnInitialized()
        {
            eventBus.Subscribe<CardCostChanged>(OnCardCostChanged);
            eventBus.Subscribe<CardReflectionChanged>(OnCardReflectionChanged);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<CardCostChanged>(OnCardCostChanged);
            eventBus?.Unsubscribe<CardReflectionChanged>(OnCardReflectionChanged);
        }
        public void Initialize(Card card, Action<BattleCardView> onCardClicked)
        {
            this.onCardClicked = onCardClicked;
            sharedCardView.SetCard(card);
            sharedCardView.UnlinkSync();
        }

        private void OnCardCostChanged(CardCostChanged payload)
        {
            if (payload.Card != sharedCardView.Card) { return;}

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CardCostChanged_UpdateView, CardCostChangedPresentation(), () =>
            {
                sharedCardView.DrawCost(payload.CurrentCost);
            });
        }
        private IEnumerator CardCostChangedPresentation()
        {
            yield return null;
        }

        private void OnCardReflectionChanged(CardReflectionChanged payload)
        {
            if (payload.Card != sharedCardView.Card) { return;}

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CardReflectionChanged_UpdateView, CardReflectionChangedPresentation(), () =>
            {
                sharedCardView.DrawDescription(payload.IsReflection);
            });
        }
        private IEnumerator CardReflectionChangedPresentation()
        {
            yield return null;
        }

        public void SetLayoutTransform(Vector3 targetLocalPos, Vector3 targetLocalAngles, Vector3 targetLocalScale)
        {
            baseLocalPosition = targetLocalPos;
            baseLocalEulerAngles = targetLocalAngles;
            baseLocalScale = targetLocalScale;

            rectTransform.anchoredPosition = baseLocalPosition;
            transform.localEulerAngles = baseLocalEulerAngles;
            transform.localScale = baseLocalScale;
        }

        public void SetBaseLayoutTransform(Vector3 basePos, Vector3 baseAngles, Vector3 baseScale)
        {
            baseLocalPosition = basePos;
            baseLocalEulerAngles = baseAngles;
            baseLocalScale = baseScale;
        }

        public void SetWorldTransform(Vector3 worldPos, Quaternion worldRot, Vector3 localScale)
        {
            rectTransform.position = worldPos;
            rectTransform.rotation = worldRot;
            rectTransform.localScale = localScale;
        }

        public void SetAlpha(float value)
        {
            canvasGroup.alpha = value;
        }

        public Tween PlayProcessMoveToLayoutTransform()
        {
            currentTween?.Kill();
            var sequence = DOTween.Sequence();
            sequence.Join(rectTransform.DOAnchorPos(baseLocalPosition, processMoveDuration).SetEase(processMoveEase));
            sequence.Join(transform.DORotate(baseLocalEulerAngles, processRotateDuration, RotateMode.Fast).SetEase(processRotateEase));
            sequence.Join(transform.DOScale(baseLocalScale, processMoveDuration).SetEase(processScaleEase));
            
            currentTween = sequence;
            return currentTween;
        }

        public Tween PlayTriggerFadePresentation()
        {
            currentTween?.Kill();
            canvasGroup.alpha = 0;
            currentTween = canvasGroup.DOFade(1, triggerFadeDuration).SetEase(triggerFadeEase);
            return currentTween;
        }

        public Tween PlayResolveFadePresentation()
        {
            currentTween?.Kill();
            canvasGroup.alpha = 1;
            currentTween = canvasGroup.DOFade(0, resolveFadeDuration).SetEase(resolveFadeEase);
            return currentTween;
        }

        public Tween PlayMoveToLayoutTransform(float moveDuration, float rotateDuration, Ease moveEase, Ease rotateEase)
        {
            currentTween?.Kill();

            var sequence = DOTween.Sequence();
            sequence.Join(rectTransform.DOAnchorPos(baseLocalPosition, moveDuration).SetEase(moveEase));
            sequence.Join(transform.DORotate(baseLocalEulerAngles, rotateDuration, RotateMode.Fast).SetEase(rotateEase));
            sequence.Join(transform.DOScale(baseLocalScale, moveDuration).SetEase(moveEase));
            
            currentTween = sequence;
            return currentTween;
        }

        public Tween PlayDrawPresentation(float moveDuration, float rotateDuration, float scaleDuration, Ease moveEase, Ease rotateEase, Ease scaleEase)
        {
            currentTween?.Kill();

            var sequence = DOTween.Sequence();
            sequence.Join(rectTransform.DOAnchorPos(baseLocalPosition, moveDuration).SetEase(moveEase));
            sequence.Join(transform.DORotate(baseLocalEulerAngles, rotateDuration, RotateMode.Fast).SetEase(rotateEase));
            sequence.Join(transform.DOScale(baseLocalScale, scaleDuration).SetEase(scaleEase));

            currentTween = sequence;
            return currentTween;
        }

        public Tween PlayDiscardPresentation(Vector3 endPos, Vector3 controlPos, Vector3 endRot, float moveDuration, float rotateDuration, float scaleDuration, Ease moveEase, Ease rotateEase, Ease scaleEase)
        {
            currentTween?.Kill();

            var sequence = DOTween.Sequence();
            Vector3 startPos = rectTransform.position;
            float t = 0f;

            sequence.Join(DOTween.To(() => t, x =>
            {
                t = x;
                rectTransform.position = CalculateQuadraticBezierPoint(t, startPos, controlPos, endPos);
            }, 1f, moveDuration).SetEase(moveEase));

            sequence.Join(rectTransform.DORotate(endRot, rotateDuration, RotateMode.Fast).SetEase(rotateEase));
            sequence.Join(rectTransform.DOScale(0.2f, scaleDuration).SetEase(scaleEase));

            currentTween = sequence;
            return currentTween;
        }

        private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            
            Vector3 p = uu * p0; 
            p += 2 * u * t * p1; 
            p += tt * p2;        
            
            return p;
        }

        public Tween PlayRestorePresentation(Vector3 targetPos, float moveDuration, Ease moveEase)
        {
            currentTween = transform.DOMove(targetPos, moveDuration).SetEase(moveEase);
            return currentTween;
        }

        public Tween PlayFadePresentation(float duration, Ease ease, bool isFadeIn = true)
        {
            canvasGroup.alpha = isFadeIn ? 0 : 1;

            currentTween = canvasGroup.DOFade(isFadeIn ? 1 : 0, duration).SetEase(ease);
            return currentTween;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            currentTween?.Kill();
            onCardClicked?.Invoke(this);
        }

        public void Focus()
        {
            currentTween?.Kill();
            
            Vector3 tiltDirection = transform.localRotation * Vector3.up;
            Vector3 targetPos = baseLocalPosition + (tiltDirection * popUpDistance);
            
            currentTween = rectTransform.DOAnchorPos(targetPos, focusMoveDuration).SetEase(focusMoveEase);
        }

        public void Unfocus()
        {
            currentTween?.Kill();
            
            currentTween = rectTransform.DOAnchorPos(baseLocalPosition, focusMoveDuration).SetEase(focusMoveEase);
        }
    }
}