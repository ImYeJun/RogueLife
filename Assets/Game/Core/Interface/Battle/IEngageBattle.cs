using System;
using System.Collections.Generic;

public interface IEngageBattle
{
    public void EngageBattle(Player player, List<EnemyDataSlot> engagingEnemiesDataSlot, int startPhaseCount, Action<BattleResult> battleExit);
}