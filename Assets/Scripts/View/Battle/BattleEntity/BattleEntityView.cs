using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using View.Core;
using ViewEvent.BattleView;

namespace View.BattleView
{
    public abstract class BattleEntityView<T> : ViewBehaviour<IBattleViewEvent>, IInspectable where T : IReadOnlyBattleEntity
    {
        [Header("BattleEntityView")]
        [SerializeField] protected BattleViewTransitionManager viewTransitionManager;
        [SerializeField] protected GameObject whole;
        [SerializeField] protected GameObject body;
        [SerializeField] protected GameObject HealthBar;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject battleStatusEffectIconPrefab;
        [SerializeField] protected Transform battleStatusEffectIconContainer;
        private List<BattleStatusEffectIcon> battleStatusEffectIcons = new List<BattleStatusEffectIcon>();
        
        protected T entity;

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

        public abstract IEnumerator PlayHurtPresentation();
        public abstract IEnumerator PlayActionPresentation();
        public virtual void OnInspect(IInspectorBuilder builder, RectTransform parent)
        {
            var subPanel = builder.AddSubPanel(parent);
            subPanel.Header = "버프/디버프";

            foreach (var icon in battleStatusEffectIcons)
            {
                icon.OnInspect(builder, subPanel.ItemContainer);
            }
        }
    }
}