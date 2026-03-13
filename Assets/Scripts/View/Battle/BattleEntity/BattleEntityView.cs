using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
        [SerializeField] protected Transform battleStatusEffectIconContainer;
        
        private List<BattleStatusEffectIcon> battleStatusEffectIcons = new List<BattleStatusEffectIcon>();

        protected T entity;
        private bool isCardTargetable;
        private Action<T> onClickedCallback;

        public override void OnInitialized()
        {
            eventBus.Subscribe<BattleStatusEffectApplied>(OnStatusEffectApplied);
            eventBus.Subscribe<BattleStatusEffectRemoved>(OnStatusEffectRemoved);
            eventBus.Subscribe<BattleStatusEffectChanged>(OnStatusEffectChanged);
            eventBus.Subscribe<BattleStatusEffectExecuted>(OnStatusEffectExecuted);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<BattleStatusEffectApplied>(OnStatusEffectApplied);
            eventBus?.Unsubscribe<BattleStatusEffectRemoved>(OnStatusEffectRemoved);
            eventBus?.Unsubscribe<BattleStatusEffectChanged>(OnStatusEffectChanged);
            eventBus?.Unsubscribe<BattleStatusEffectExecuted>(OnStatusEffectExecuted);
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

        private void OnStatusEffectExecuted(BattleStatusEffectExecuted payload)
        {
            if (!payload.Owner.Equals(entity)) { return; }

            var iconView = battleStatusEffectIcons.FirstOrDefault(icon => icon.CurrentEffect == payload.BattleStatusEffect);

            if (iconView is null)
            {
                throw new InvalidCastException($"[BattleEntityView] Given Entity({gameObject.name}) doesn't contain battle status effect({payload.BattleStatusEffect.Data.Name}) but try to execute.");
            }

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.BattleStatusEffectExecuted_IconAction, StatusEffectExectuedPresentation(payload));
        }
        private IEnumerator StatusEffectExectuedPresentation(BattleStatusEffectExecuted payload)
        {
            Debug.Log($"{payload.BattleStatusEffect.Data.Name} 효과 실행 됨");
            yield return new WaitForSeconds(1.0f);
        }

        public void OnCardTargetable(Action<T> onClicked)
        {
            isCardTargetable = true;
            onClickedCallback = onClicked;
            
            // TODO: 외곽선 하이라이트 연출 ON
            spriteRenderer.color = Color.black;
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

        public abstract IEnumerator PlayHurtPresentation();
        public abstract IEnumerator PlayActionPresentation();
    }
}