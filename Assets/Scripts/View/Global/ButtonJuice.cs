using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace View.Global
{
    public class ButtonJuice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
        [Header("Settings")]
        [SerializeField] private bool isScaleAble = true;
        [SerializeField] private AudioData clickSound;
        // [SerializeField] private Transform specificScaleTarget;

        [Header("Scale Presentation")]
        [SerializeField] private float focusingPresentationDuration = 0.1f;
        [SerializeField, FormerlySerializedAs("focusingScale")] private float focusingScaleMultiplier = 1.05f;
        [SerializeField] private Ease focusingPresentationEase = Ease.InOutQuad;
        
        private Tween currentFocusingTween;
        private Vector3 originalScale;
        private void Awake()
        {
            originalScale = transform.localScale;
        }

        private void OnDisable()
        {
            currentFocusingTween?.Kill();
            transform.localScale = originalScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SoundManager.Instance?.PlaySoundEffectWithRandomPitch(clickSound);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isScaleAble) { return; }

            var scaleTarget = transform;
            // if (specificScaleTarget is not null) { scaleTarget = specificScaleTarget; }

            Vector3 targetScale = originalScale * focusingScaleMultiplier;
            currentFocusingTween?.Kill();
            currentFocusingTween = scaleTarget.DOScale(targetScale, CalculateFocusingDuration(scaleTarget.localScale.x, targetScale.x))
                .SetEase(focusingPresentationEase);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isScaleAble) { return; }

            var scaleTarget = transform;
            // if (specificScaleTarget is not null) { scaleTarget = specificScaleTarget; }

            Vector3 targetScale = originalScale;
            currentFocusingTween?.Kill();
            currentFocusingTween = scaleTarget.DOScale(targetScale, CalculateFocusingDuration(scaleTarget.localScale.x, targetScale.x))
                .SetEase(focusingPresentationEase);
        }

        private float CalculateFocusingDuration(float currentScaleX, float targetScaleX)
        {
            float maxDelta = Mathf.Abs((originalScale.x * focusingScaleMultiplier) - originalScale.x);
            
            float currentDelta = Mathf.Abs(targetScaleX - currentScaleX);

            float ratio = maxDelta == 0 ? 0 : currentDelta / maxDelta;

            return focusingPresentationDuration * ratio;
        }
    }
}