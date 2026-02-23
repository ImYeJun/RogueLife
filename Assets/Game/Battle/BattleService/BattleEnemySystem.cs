using System;
using System.Collections.Generic;
using System.Linq;

public class BattleEnemySystem : IBattleEnemySystemContext, IBattleEventObserveService
{
    private BattleContext context;
    private BattleEnemyHistory history;
    private Dictionary<EnemyData, List<BattleEnemy>> currentEnemies = new Dictionary<EnemyData, List<BattleEnemy>>();

    public BattleEnemyHistory History { get => history; }
    
    public void SetContext(BattleContext context) { this.context = context; }

    public void SpawnEnemy(BattleEnemy enemy)
    {
        EnemyData data = enemy.Data;

        if (!currentEnemies.ContainsKey(data)) { currentEnemies.Add(data, new List<BattleEnemy>()); }

        currentEnemies[data].Insert(0, enemy);
        enemy.Died += RemoveEnemy;
    }
    
    public void RemoveEnemy(BattleEnemy enemy)
    {
        if (!currentEnemies.ContainsKey(enemy.Data)) { throw new InvalidOperationException($"There's not enemy data for {enemy.Data.EnemyName}"); }

        var enemyList = currentEnemies[enemy.Data];

        if (!enemyList.Remove(enemy))
        {
            throw new InvalidOperationException("There is no enemy for given the argument");
        }
        enemy.Died -= RemoveEnemy;

        if (currentEnemies[enemy.Data].Count == 0) { currentEnemies.Remove(enemy.Data); }
        if (currentEnemies.Count == 0) { 
            var action = new RequestBattleEndBattleAction(BattleResult.PLAYER_WIN);
            context.ActionScheduler.EnqueueFront(action);
        }
    }

    public List<BattleEnemy> GetBattleEnemies()
    {
        return currentEnemies.Values.SelectMany(set => set).ToList();
    }

    public int GetEnemyCountByData(EnemyData data)
    {
        return currentEnemies.ContainsKey(data) ? currentEnemies[data].Count : 0;
    }
    public void NullifyActionIfStunned(BattleEntityAction battleEntityAction, BattleContext context)
    {
        var actor = battleEntityAction.Actor;
        var enemyList = currentEnemies.Values.SelectMany(sel => sel);
        if (enemyList.Contains(actor))
        {
            if (actor.CurrentCondition.HasFlag(BattleEntityCondition.STUNNED))
            {
                battleEntityAction.Nullify();
            }
        }
    }
    
    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        eventBus.Subscribe<BattleStartEvent>(Initiate);
        eventBus.Subscribe<PhaseStartBattleEvent>(PlanNextEnemyAction);
        eventBus.Subscribe<EnemyTurnStartBattleEvent>(ExecuteEnemyAction);
        eventBus.Subscribe<BattleEndBattleEvent>(OnBattleEnd);
    }
    public void Initiate(BattleStartEvent payload)
    {
        currentEnemies.Clear();
        foreach (var enemy in payload.Enemies)
        {
            // var spawnEnemyAction = new SpawnEnemyBattleAction(enemy);
            // context.ActionScheduler.Enqueue(spawnEnemyAction);
            SpawnEnemy(enemy);
        }

        context.ActionObserverHub.SubscribeActionModifier<BattleEntityAction>(NullifyActionIfStunned);
    }
    public void PlanNextEnemyAction(PhaseStartBattleEvent payload)
    {
        foreach (var enemyList in currentEnemies.Values)
        {
            foreach (var enemy in enemyList)
            {
                enemy.PlanNextAction();
            }
        }
    }
    public void ExecuteEnemyAction(EnemyTurnStartBattleEvent payload)
    {
        foreach (var enemyGroup in currentEnemies.Values)
        {
            for (int i = enemyGroup.Count - 1; i >= 0; i--)
            {
                var enemy = enemyGroup[i];
                var plannedActions = enemy.PlannedActions;

                foreach (var actionData in plannedActions)
                {
                    var executeAction = new ExecuteEnemyActionBattleAction(actionData);
                    context.ActionScheduler.Enqueue(new BattleEntityAction(enemy, executeAction));
                }
            }
        }

        context.BattleScheduler.EndEnemyTurn();
    }
    public void OnBattleEnd(BattleEndBattleEvent payload)
    {
        context.ActionObserverHub.UnsubscribeActionModifier<BattleEntityAction>(NullifyActionIfStunned);
    }
}

