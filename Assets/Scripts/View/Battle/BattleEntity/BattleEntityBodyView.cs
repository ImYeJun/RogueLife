using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.BattleView
{
    public abstract class BattleEntityBodyView<T> : MonoBehaviour, IPointerClickHandler where T : IReadOnlyBattleEntity
    {
        [SerializeField] protected SpriteRenderer spriteRenderer;

        protected T entity;
        private IInspectable inspectableEntity; 
        private bool isCardTargetable;
        private Action<T> onCardTargetedClickedCallback;

        private Action<IInspectable, Transform, BattleEntityInspectorView.InspectorDirection> onEntityInspectClickedCallback;
        private BattleEntityInspectorView.InspectorDirection inspectorDirection;

        public virtual void Initialize(T entity, IInspectable inspectableEntity, Action<IInspectable, Transform, BattleEntityInspectorView.InspectorDirection> onEntityInspectClickedCallback,  BattleEntityInspectorView.InspectorDirection inspectorDirection)
        {
            this.entity = entity;
            this.inspectableEntity = inspectableEntity;
            this.onEntityInspectClickedCallback = onEntityInspectClickedCallback;
            this.inspectorDirection = inspectorDirection;
        }

        public void OnCardTargetable(Action<T> onTargetClicked)
        {
            isCardTargetable = true;
            onCardTargetedClickedCallback = onTargetClicked;
            
            spriteRenderer.color = Color.black;
        }

        public void OnCardUntargetable()
        {
            isCardTargetable = false;
            onCardTargetedClickedCallback = null;
            
            spriteRenderer.color = Color.white;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (entity == null)
            {
                throw new InvalidOperationException("[BattleEntityBodyView] Entity is not initialized");
            }

            if (isCardTargetable && onCardTargetedClickedCallback != null)
            {
                onCardTargetedClickedCallback.Invoke(entity);
            }
            else
            {
                onEntityInspectClickedCallback.Invoke(inspectableEntity, transform, inspectorDirection);
            }
        }

        public abstract void SetActionSprite();
        public abstract void SetIdleSprite();
    }
}