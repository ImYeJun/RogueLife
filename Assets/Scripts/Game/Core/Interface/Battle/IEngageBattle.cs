using System;
using System.Collections.Generic;
using Battle.BattleResultCommands;

public interface IEngageBattle
{
    public void EngageBattle(IBattleHealth battleHealth, IBattleEntryActionCost actionCost, IBattleEntryDeck deck, IBattleEntryBelongingsBag belongingsBag, List<EnemyDataSlot> engagingEnemiesDataSlot, Action<BattleResultCommand> battleExit);
}