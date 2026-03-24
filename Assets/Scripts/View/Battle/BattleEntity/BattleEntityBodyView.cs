using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.BattleView
{
    public abstract class BattleEntityBodyView<T> : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler where T : IReadOnlyBattleEntity
    {
        [SerializeField] protected SpriteRenderer spriteRenderer;

        [Header("Materials")]
        [SerializeField] private Material targetableMaterial;
        [SerializeField] private Material targetableHoverMaterial;
        private Material originalMaterial;

        [Header("Presentation")]
        [SerializeField] private float focusingMultiplayAmount;
        [SerializeField] private float focusingPresentationDuration;
        [SerializeField] private Ease focusingPresentationEase;
        private float FocusingScale => focusingMultiplayAmount * originalScale;
        private float originalScale;
        private Tween currentFocusingTween;

        protected T entity;
        private IInspectable inspectableEntity; 
        private bool isCardTargetable;
        private Action<T> onCardTargetedClickedCallback;

        private Action<IInspectable, Transform, BattleEntityInspectorView.InspectorDirection> onEntityInspectClickedCallback;
        private BattleEntityInspectorView.InspectorDirection inspectorDirection;

        public virtual void Initialize(T entity, IInspectable inspectableEntity, Action<IInspectable, Transform, BattleEntityInspectorView.InspectorDirection> onEntityInspectClickedCallback,  BattleEntityInspectorView.InspectorDirection inspectorDirection)
        {
            originalScale = transform.localScale.x;
            originalMaterial = spriteRenderer.material;

            this.entity = entity;
            this.inspectableEntity = inspectableEntity;
            this.onEntityInspectClickedCallback = onEntityInspectClickedCallback;
            this.inspectorDirection = inspectorDirection;
        }

        public void OnCardTargetable(Action<T> onTargetClicked)
        {
            isCardTargetable = true;
            onCardTargetedClickedCallback = onTargetClicked;
            
            spriteRenderer.material = targetableMaterial;
        }

        public void OnCardUntargetable()
        {
            isCardTargetable = false;
            onCardTargetedClickedCallback = null;
            
            spriteRenderer.material = originalMaterial;
            ResetScale();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (entity == null)
            {
                throw new InvalidOperationException("[BattleEntityBodyView] Entity is not initialized");
            }

            ResetScale();

            if (isCardTargetable && onCardTargetedClickedCallback != null)
            {
                onCardTargetedClickedCallback.Invoke(entity);
            }
            else
            {
                onEntityInspectClickedCallback.Invoke(inspectableEntity, transform, inspectorDirection);
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (entity == null) return;

            if (isCardTargetable)
            {
                spriteRenderer.material = targetableHoverMaterial;
            }

            currentFocusingTween?.Kill();
            currentFocusingTween = transform.DOScale(FocusingScale, CalculateFocusingDuration(transform.localScale.x)).SetEase(focusingPresentationEase);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (entity == null) return;

            if (isCardTargetable)
            {
                spriteRenderer.material = targetableMaterial;
            }

            ResetScale();
        }

        private void ResetScale()
        {
            currentFocusingTween?.Kill();
            currentFocusingTween = transform.DOScale(originalScale, CalculateFocusingDuration(transform.localScale.x)).SetEase(focusingPresentationEase);
        }

        private float CalculateFocusingDuration(float currentScale)
        {
            float originalDelta = Mathf.Abs(FocusingScale - originalScale);
            float currentDelta = Mathf.Abs(FocusingScale - currentScale);

            float ratio = originalDelta == 0 ? 0 : currentDelta / originalDelta;

            return focusingPresentationDuration * ratio;
        }

        public abstract void SetActionSprite();
        public abstract void SetIdleSprite();
    }
}