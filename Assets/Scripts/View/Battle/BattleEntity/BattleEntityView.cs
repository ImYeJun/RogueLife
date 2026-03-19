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
    public abstract class BattleEntityView<T> : InteractableViewBehaviour<IBattleViewEvent, IBattleViewCommander>, IInspectable where T : IReadOnlyBattleEntity
    {
        [Header("BattleEntityView")]
        [SerializeField] protected BattleViewTransitionManager viewTransitionManager;
        [SerializeField] protected GameObject whole;
        [SerializeField] protected GameObject body;
        [SerializeField] protected GameObject HealthBar;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        
        [Header("Fade Presentation Settings")]
        [SerializeField] protected float fadeDuration = 0.5f; // 💡 [추가됨] 등장/퇴장 페이드 시간

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

        // 💡 [추가됨] 즉각적인 은신 (초기화 직후 호출)
        public void SetInvisibleDirectly()
        {
            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = 0f;
                spriteRenderer.color = c;
            }
            
            // UI 요소나 자식 캔버스가 있다면 필요시 CanvasGroup을 통해 0으로 맞춰야 합니다.
            // 여기서는 spriteRenderer 기준으로만 처리합니다.
        }

        // 💡 [추가됨] 서서히 나타나는 등장 연출 트윈 반환
        public Tween PlayAppearPresentation()
        {
            if (spriteRenderer != null)
            {
                return spriteRenderer.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad);
            }
            return DOTween.Sequence();
        }

        // 💡 [추가됨] 서서히 사라지는 퇴장 연출 트윈 반환
        public Tween PlayDisappearPresentation()
        {
            if (spriteRenderer != null)
            {
                return spriteRenderer.DOFade(0f, fadeDuration).SetEase(Ease.InQuad);
            }
            return DOTween.Sequence();
        }

        private void OnStatusEffectApplied(BattleStatusEffectApplied payload)
        {
            if (!payload.Entity.Equals(entity)) { return; }

            GameObject iconObj = Instantiate(battleStatusEffectIconPrefab, battleStatusEffectIconContainer);
            BattleStatusEffectIcon icon = iconObj.GetComponent<BattleStatusEffectIcon>();

            icon.Initialize(payload.BattleStatusEffect);
            battleStatusEffectIcons.Add(icon);

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.BattleStatusEffectAplied_IconAction, icon.PlayAppliedPresentation());
        }

        private void OnStatusEffectRemoved(BattleStatusEffectRemoved payload)
        {
            if (entity == null || !payload.Entity.Equals(entity)) { return; }

            BattleStatusEffectIcon iconToRemove = battleStatusEffectIcons.FirstOrDefault(icon => icon.CurrentEffect.Equals(payload.BattleStatusEffect));
            
            if (iconToRemove != null)
            {
                battleStatusEffectIcons.Remove(iconToRemove);
                presentationManager.Enqueue(payload.SequenceId, PresentationPriority.BattleStatusEffectRemoved_IconAction, iconToRemove.PlayRemovedPresentation(),
                () =>
                {
                    Destroy(iconToRemove.gameObject);
                });
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

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.BattleStatusEffectExecuted_IconAction, iconView.PlayExectuedPresentation());
        }

        public virtual void OnInspect(IInspectorBuilder builder, RectTransform parent)
        {
            var subPanel = builder.AddSubPanel(parent);
            subPanel.Header = "보유 중인 버프·디버프";

            foreach (var icon in battleStatusEffectIcons)
            {
                icon.OnInspect(builder, subPanel.ItemContainer);
            }
        }
    }
}