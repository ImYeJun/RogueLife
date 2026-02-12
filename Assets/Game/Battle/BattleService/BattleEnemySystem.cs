using System;
using System.Collections.Generic;
using System.Linq;

public class BattleEnemySystem : IBattleEnemySystemContext, IBattleEventObserver
{
    private BattleContext context;
    private BattleEnemyHistory history;
    private Dictionary<EnemyData, List<BattleEnemy>> currentEnemies = new Dictionary<EnemyData, List<BattleEnemy>>();

    public void SpawnEnemy(BattleEnemy enemy)
    {
        EnemyData data = enemy.Data;

        if (!currentEnemies.ContainsKey(data)) { currentEnemies.Add(data, new List<BattleEnemy>()); }

        currentEnemies[data].Insert(0, enemy);
    }

    public List<BattleEnemy> GetBattleEnemies()
    {
        return currentEnemies.Values.SelectMany(set => set).ToList();
    }

    public int GetEnemyCountByData(EnemyData data)
    {
        return currentEnemies.ContainsKey(data) ? currentEnemies[data].Count : 0;
    }

    public void OnBattleEvent(BattleEvent battleEvent)
    {
        if (battleEvent is PhaseStartBattleEvent)
        {
            foreach (var enemyList in currentEnemies.Values)
            {
                foreach (var enemy in enemyList)
                {
                    enemy.PlanNextAction();
                }
            }
        }

        if (battleEvent is EnemyTurnStartBattleEvent)
        {
            foreach (var enemyList in currentEnemies.Values)
            {
                for (int i = enemyList.Count - 1; i >= 0; i--)
                {
                    var enemy = enemyList[i];
                    var actions = enemy.PlannedActions;

                    foreach (var action in actions)
                    {
                        context.ActionScheduler.Enqueue(new BattleEntityAction(enemy, action));
                    }
                }
            }

            context.BattleScheduler.EndEnemyTurn();
        }
    }
}

