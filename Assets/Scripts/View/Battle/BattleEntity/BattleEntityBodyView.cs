using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.BattleView
{
    public abstract class BattleEntityBodyView<T> : MonoBehaviour, IPointerClickHandler where T : IReadOnlyBattleEntity
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        protected T entity;
        private bool isCardTargetable;
        private Action<T> onTargetClickedCallback;

        public virtual void Initialize(T entity)
        {
            this.entity = entity;
        }

        public void OnCardTargetable(Action<T> onTargetClicked)
        {
            isCardTargetable = true;
            onTargetClickedCallback = onTargetClicked;
            
            // TODO: 외곽선 하이라이트 연출 ON
            spriteRenderer.color = Color.black;
        }

        public void OnCardUntargetable()
        {
            isCardTargetable = false;
            onTargetClickedCallback = null;
            
            // TODO: 외곽선 하이라이트 연출 OFF
            spriteRenderer.color = Color.white;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (entity == null)
            {
                throw new InvalidOperationException("[BattleEntityBodyView] Entity is not initialized");
            }

            if (isCardTargetable && onTargetClickedCallback != null)
            {
                onTargetClickedCallback.Invoke(entity);
            }
            else
            {
                
            }
        }
    }
}