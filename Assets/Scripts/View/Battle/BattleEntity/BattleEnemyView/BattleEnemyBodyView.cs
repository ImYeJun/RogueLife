using System;
using UnityEngine;

namespace View.BattleView
{
    public class BattleEnemyBodyView : BattleEntityBodyView<IReadOnlyBattleEnemy> {
        public override void Initialize(IReadOnlyBattleEnemy entity, IInspectable inspectableEntity, Action<IInspectable, Transform, BattleEntityInspectorView.InspectorDirection> onEntityInspectClickedCallback,  BattleEntityInspectorView.InspectorDirection inspectorDirection)
        {
            base.Initialize(entity, inspectableEntity, onEntityInspectClickedCallback,inspectorDirection);

            SetIdleSprite();
        }

        public void SetSpriteSortingOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
        }

        public override void SetActionSprite()
        {
            spriteRenderer.sprite = entity.Data.GetBattleSprite(EnemySpriteType.Action);
        }

        public override void SetIdleSprite()
        {
            spriteRenderer.sprite = entity.Data.GetBattleSprite(EnemySpriteType.Idle);
        }
    }
}