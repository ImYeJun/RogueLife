using System;
using UnityEngine;

namespace View.BattleView
{
    public class BattlePlayerBodyView : BattleEntityBodyView<IReadOnlyBattlePlayer> {
        [SerializeField] private Sprite idleSprite;
        [SerializeField] private Sprite actionSprite;

        public IReadOnlyBattlePlayer Player => entity;
        public override void Initialize(IReadOnlyBattlePlayer entity, IInspectable inspectableEntity, Action<IInspectable, Transform, BattleEntityInspectorView.InspectorDirection> onEntityInspectClickedCallback,  BattleEntityInspectorView.InspectorDirection inspectorDirection)
        {
            base.Initialize(entity, inspectableEntity, onEntityInspectClickedCallback,inspectorDirection);
        }

        public override void SetActionSprite()
        {
            spriteRenderer.sprite = actionSprite;
        }

        public override void SetIdleSprite()
        {
            spriteRenderer.sprite = idleSprite;
        }
    } 
}