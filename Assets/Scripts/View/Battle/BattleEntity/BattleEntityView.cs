using System;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Core;
using ViewEvent.BattleView;

namespace View.BattleView
{
    public abstract class BattleEntityView<T> : ViewBehaviour<IBattleViewEvent>, IPointerClickHandler where T : IReadOnlyBattleEntity
    {
        [SerializeField] SpriteRenderer spriteRenderer;

        protected T entity;
        private bool isCardTargetable;
        private Action<T> onClickedCallback;

        public void OnCardTargetable(Action<T> onClicked)
        {
            isCardTargetable = true;
            onClickedCallback = onClicked;
            
            // TODO: 외곽선 하이라이트 연출 ON

            spriteRenderer.color = Color.red;
        }

        public void OnCardUntargetable()
        {
            isCardTargetable = false;
            onClickedCallback = null;
            
            // TODO: 외곽선 하이라이트 연출 OFF
            spriteRenderer.color = Color.white;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isCardTargetable && onClickedCallback != null)
            {
                if (entity is null)
                {
                    throw new InvalidOperationException("[BattleEntityView] entity is not initialized in sub-class");
                }

                onClickedCallback.Invoke(entity);
            }
        }
    }
}