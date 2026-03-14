using System;
using UnityEngine;

namespace View.BattleView
{
    public class BattlePlayerBodyView : BattleEntityBodyView<IReadOnlyBattlePlayer> {
        public IReadOnlyBattlePlayer Player => entity;
        public override void Initialize(IReadOnlyBattlePlayer entity, IInspectable inspectableEntity, Action<IInspectable> onEntityInspectClickedCallback)
        {
            base.Initialize(entity, inspectableEntity, onEntityInspectClickedCallback);
        }
    }
}