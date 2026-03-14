using System;
using UnityEngine;

namespace View.BattleView
{
    public class BattlePlayerBodyView : BattleEntityBodyView<IReadOnlyBattlePlayer> {
        public IReadOnlyBattlePlayer Player => entity;
        public override void Initialize(IReadOnlyBattlePlayer entity, IInspectable inspectableEntity, Action<IInspectable, Transform, BattleEntityInspectorView.InspectorDirection> onEntityInspectClickedCallback,  BattleEntityInspectorView.InspectorDirection inspectorDirection)
        {
            base.Initialize(entity, inspectableEntity, onEntityInspectClickedCallback,inspectorDirection);
        }
    } 
}