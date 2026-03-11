using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Core;
using ViewEvent.BattleView;

namespace View.BattleView
{
    public abstract class BattleEntityView<T> : ViewBehaviour<IBattleViewEvent>, IPointerClickHandler where T : IReadOnlyBattleEntity
    {
        [Header("BattleEntityView")]
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject battleStatusEffectIconPrefab;
        [SerializeField] private Transform battleStatusEffectIconContainer;
        
        private List<BattleStatusEffectIcon> battleStatusEffectIcons = new List<BattleStatusEffectIcon>();

        protected T entity;
        private bool isCardTargetable;
        private Action<T> onClickedCallback;

        public override void OnInitialized()
        {
            eventBus.Subscribe<BattleStatusEffectApplied>(OnStatusEffectApplied);
            eventBus.Subscribe<BattleStatusEffectRemoved>(OnStatusEffectRemoved);
            eventBus.Subscribe<BattleStatusEffectChanged>(OnStatusEffectChanged);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<BattleStatusEffectApplied>(OnStatusEffectApplied);
            eventBus?.Unsubscribe<BattleStatusEffectRemoved>(OnStatusEffectRemoved);
            eventBus?.Unsubscribe<BattleStatusEffectChanged>(OnStatusEffectChanged);
        }

        private void OnStatusEffectApplied(BattleStatusEffectApplied payload)
        {
            if (!payload.Entity.Equals(entity)) { return; }

            GameObject iconObj = Instantiate(battleStatusEffectIconPrefab, battleStatusEffectIconContainer);
            BattleStatusEffectIcon icon = iconObj.GetComponent<BattleStatusEffectIcon>();

            icon.Initialize(payload.BattleStatusEffect);
            battleStatusEffectIcons.Add(icon);
        }

        private void OnStatusEffectRemoved(BattleStatusEffectRemoved payload)
        {
            if (entity == null || !payload.Entity.Equals(entity)) { return; }

            BattleStatusEffectIcon iconToRemove = battleStatusEffectIcons.FirstOrDefault(icon => icon.CurrentEffect.Equals(payload.BattleStatusEffect));
            
            if (iconToRemove != null)
            {
                battleStatusEffectIcons.Remove(iconToRemove);
                Destroy(iconToRemove.gameObject);
            }
            else
            {
                throw new InvalidOperationException("[BattleEntityView] Received status effect removed event for an untracked status effect.");
            }
        }

        private void OnStatusEffectChanged(BattleStatusEffectChanged payload)
        {
            if (entity == null || !payload.Entity.Equals(entity)) { return; }

            BattleStatusEffectIcon iconToUpdate = battleStatusEffectIcons.FirstOrDefault(icon => icon.CurrentEffect.Equals(payload.BattleStatusEffect));
            
            if (iconToUpdate != null)
            {
                iconToUpdate.UpdateState(payload.RemainTurn, payload.CurrentStack);
            }
            else
            {
                throw new InvalidOperationException("[BattleEntityView] Received changed event for an untracked status effect.");
            }
        }

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
                if (entity == null)
                {
                    throw new InvalidOperationException("[BattleEntityView/OnPointerClick] Entity is not initialized in sub-class.");
                }

                onClickedCallback.Invoke(entity);
            }
        }
    }
}