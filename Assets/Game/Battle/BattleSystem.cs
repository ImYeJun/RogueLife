using System;
using System.Collections.Generic;

public class BattleSystem
{
    private BattleContext context;
    private BattleEventBus eventBus;
    private BattleScheduler scheduler;
    private BattleActionPipeline pipeline;
    private BattleDeckSystem deckSystem;
    private BattleActionCost acionCost;
    private BattleEnemySystem enemySystem;

    public event Action<BattleResult> OnBattleExit;

    public void EngageBattle(List<EnemyData> engagingEnemiesData, int startPhaseCount, Action<BattleResult> onBattleExit)
    {
        throw new NotImplementedException();
    }

    public void ExitBattle(BattleResult result)
    {
        OnBattleExit?.Invoke(result);
    }
}