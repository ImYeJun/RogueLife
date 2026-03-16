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

        private Action<BattleCardView> onCardClicked; 
        private Vector3 baseLocalPosition;
        private Vector3 baseLocalEulerAngles;

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

        public Tween MoveToLayoutTransform(float moveDuration, float rotateDuration, Ease moveEase, Ease rotateEase)
        {
            var sequence = DOTween.Sequence();
            sequence.Join(rectTransform.DOAnchorPos(baseLocalPosition, moveDuration).SetEase(moveEase));
            sequence.Join(transform.DORotate(baseLocalEulerAngles, rotateDuration, RotateMode.Fast).SetEase(rotateEase));
            return sequence;
        }

        public Tween PlayDrawPresentation(float moveDuration, float rotateDuration, float scaleDuration, Ease moveEase, Ease rotateEase, Ease scaleEase)
        {
            var sequence = DOTween.Sequence();
            sequence.Join(rectTransform.DOAnchorPos(baseLocalPosition, moveDuration).SetEase(moveEase));
            sequence.Join(transform.DORotate(baseLocalEulerAngles, rotateDuration, RotateMode.Fast).SetEase(rotateEase));
            sequence.Join(transform.DOScale(Vector3.one, scaleDuration).SetEase(scaleEase));
            return sequence;
        }

        public Tween PlayDiscardPresentation(Vector3 endPos, Vector3 controlPos, Vector3 endRot, float moveDuration, float rotateDuration, float scaleDuration, Ease moveEase, Ease rotateEase, Ease scaleEase)
        {
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

            return sequence;
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
            return transform.DOMove(targetPos, moveDuration).SetEase(moveEase);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onCardClicked?.Invoke(this);
        }

        public void Focus()
        {
            Vector3 tiltDirection = transform.localRotation * Vector3.up;
            rectTransform.anchoredPosition = baseLocalPosition + (tiltDirection * popUpDistance);
        }

        public void Unfocus()
        {
            rectTransform.anchoredPosition = baseLocalPosition;
        }
    }
}