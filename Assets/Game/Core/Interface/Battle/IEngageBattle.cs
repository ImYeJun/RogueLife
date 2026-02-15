using System;
using System.Collections.Generic;

public interface IEngageBattle
{
    public void EngageBattle(IBattleHealth battleHealth, IBattleEntryActionCost actionCost, IBattleEntryDeck deck, IBattleEntryBelongingsBag belongingsBag, List<EnemyDataSlot> engagingEnemiesDataSlot, int startPhaseCount, Action<BattleResult> battleExit);
}