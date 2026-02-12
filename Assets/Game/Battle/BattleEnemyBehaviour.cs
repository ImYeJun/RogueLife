using System;
using System.Collections.Generic;

[Serializable]
public abstract class BattleEnemyBehaviour
{
    protected List<EnemyAction> owingActioncs;
    public abstract BattleEnemyBehaviour Clone();
}
