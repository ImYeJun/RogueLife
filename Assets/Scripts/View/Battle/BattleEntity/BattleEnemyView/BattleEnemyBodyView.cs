using System;
using UnityEngine;

namespace View.BattleView
{
    public class BattleEnemyBodyView : BattleEntityBodyView<IReadOnlyBattleEnemy> {
        public override void Initialize(IReadOnlyBattleEnemy entity)
        {
            base.Initialize(entity);
        }
    }
}