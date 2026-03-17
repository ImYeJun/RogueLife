using System;
using DG.Tweening; 
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.BattleView
{
    public class BattleCardView : MonoBehaviour, IPointerClickHandler
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

        [Header("Resolve Card Presentation")]
        [SerializeField] private float resolveFadeDuration = 0.5f;
        [SerializeField] private Ease resolveFadeEase = Ease.Linear;

        private Action<BattleCardView> onCardClicked; 
        private Vector3 baseLocalPosition;
        private Vector3 baseLocalEulerAngles;

        private Tween currentTween;

        public Card Card => sharedCardView.Card;

        public void Initialize(Card card, Action<BattleCardView> onCardClicked)
        {
            this.onCardClicked = onCardClicked;
            sharedCardView.SetCard(card);
        }

        public void SetLayoutTransform(Vector3 targetLocalPos, Vector3 targetLocalAngles)
        {
            baseLocalPosition = targetLocalPos;
            baseLocalEulerAngles = targetLocalAngles;

            rectTransform.anchoredPosition = baseLocalPosition;
            transform.localEulerAngles = baseLocalEulerAngles;
        }

        public void SetBaseLayoutTransform(Vector3 basePos, Vector3 baseAngles)
        {
            baseLocalPosition = basePos;
            baseLocalEulerAngles = baseAngles;
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
            
            currentTween = sequence;
            return currentTween;
        }

        public Tween PlayDrawPresentation(float moveDuration, float rotateDuration, float scaleDuration, Ease moveEase, Ease rotateEase, Ease scaleEase)
        {
            currentTween?.Kill();

            var sequence = DOTween.Sequence();
            sequence.Join(rectTransform.DOAnchorPos(baseLocalPosition, moveDuration).SetEase(moveEase));
            sequence.Join(transform.DORotate(baseLocalEulerAngles, rotateDuration, RotateMode.Fast).SetEase(rotateEase));
            sequence.Join(transform.DOScale(Vector3.one, scaleDuration).SetEase(scaleEase));

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