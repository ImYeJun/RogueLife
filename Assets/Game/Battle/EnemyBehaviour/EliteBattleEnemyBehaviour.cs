using System;
using System.Collections.Generic;

public abstract class EliteBattleEnemyBehaviour : BattleEnemyBehaviour
{
    protected override int CalculateActionCount(Random random)
    {
        int previousActionCount = owner.PreviousActionCount;

        if (!owner.IsFirstAction && previousActionCount == Constant.ELITE_ENEMY_MIN_BEHAVIOUR_COUNT) { 
            return Constant.ELITE_ENEMY_OVER_BEHAVIOUR_COUNT;
        }

        return random.Next(Constant.ELITE_ENEMY_MIN_BEHAVIOUR_COUNT, Constant.ELITE_ENEMY_MAX_BEHAVIOUR_COUNT + 1);
    }
}