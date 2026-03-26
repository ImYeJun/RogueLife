using System;
using System.Collections.Generic;
using System.Linq;
using ViewEvent.BattleView;

public partial class BattleEnemySystem : IBattleEnemySystemContext, IBattleEventObserveService
{
    private BattleContext context;
    private IBattleViewEventPublisher viewEventPublisher;
    private BattleEnemyHistory history;
    private Dictionary<EnemyData, List<BattleEnemy>> currentEnemies = new Dictionary<EnemyData, List<BattleEnemy>>();

    public BattleEnemySystem(IBattleViewEventPublisher viewEventPublisher)
    {
        this.viewEventPublisher = viewEventPublisher;
        history = new BattleEnemyHistory();
    }

    public bool IsAnihilated => currentEnemies.Count <= 0;
    public BattleEnemyHistory History { get => history; }
    
    public void SetContext(BattleContext context) { this.context = context; }

    private bool TryRegisterEnemy(BattleEnemy enemy)
    {
        if (currentEnemies.Values.Sum(list => list.Count) >= Constant.MAX_SPAWNED_ENEMY_COUNT) 
        { 
            return false; 
        }

        EnemyData data = enemy.Data;
        enemy.SetViewEventPublisher(viewEventPublisher);
        
        if (!currentEnemies.ContainsKey(data)) 
        { 
            currentEnemies.Add(data, new List<BattleEnemy>()); 
        }

        currentEnemies[data].Insert(0, enemy);
        enemy.Died += RemoveEnemy;

        return true;
    }

    public void SpawnEnemy(BattleEnemy enemy)
    {
        if (TryRegisterEnemy(enemy))
        {
            enemy.OnSpawned();

            viewEventPublisher.Publish(new EnemySpawned(viewEventPublisher.GetNextSequenceId(), enemy));
        }
    }
    
    public void RemoveEnemy(BattleEnemy enemy)
    {
        if (!currentEnemies.ContainsKey(enemy.Data)) { throw new InvalidOperationException($"[BattleEnemySystem/RemoveEnemy] There's not enemy data for {enemy.Data.EnemyName}"); }

        var enemyList = currentEnemies[enemy.Data];

        if (!enemyList.Remove(enemy))
        {
            throw new InvalidOperationException("[BattleEnemySystem/RemoveEnemy] There is no enemy for the given argument");
        }
        enemy.Died -= RemoveEnemy;
        viewEventPublisher.Publish(new EnemyRemoved(viewEventPublisher.GetNextSequenceId(), enemy));

        if (currentEnemies[enemy.Data].Count == 0) { currentEnemies.Remove(enemy.Data); }
        if (currentEnemies.Count == 0) { 
            var action = new RequestBattleEndBattleAction(BattleResultType.PLAYER_ANNIHILATE_WIN);
            context.ActionScheduler.EnqueueFront(action);
        }
    }

    public List<BattleEnemy> GetBattleEnemies()
    {
        return currentEnemies.Values.SelectMany(set => set).ToList();
    }
    public List<BattleEnemy> GetBattleEnemies(EnemyData data)
    {
        if (currentEnemies.TryGetValue(data, out var enemies))
        {
            return enemies;
        }
        
        return new List<BattleEnemy>();
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
        List<IReadOnlyBattleEnemy> initialEnemies = new List<IReadOnlyBattleEnemy>();

        foreach (var enemy in payload.Enemies)
        {
            if (TryRegisterEnemy(enemy))
            {
                enemy.OnSpawned();
                initialEnemies.Add(enemy);
            }
        }

        viewEventPublisher.Publish(new InitialEnemySettled(viewEventPublisher.GetNextSequenceId(), initialEnemies));

        context.ActionObserverHub.SubscribeActionModifier<BattleEntityAction>(NullifyActionIfStunned);
    }

    public void PlanNextEnemyAction(PhaseStartBattleEvent payload)
    {
        var groupSnapshot = currentEnemies.Values.ToList();
        
        foreach (var enemyList in groupSnapshot)
        {
            var enemySnapshot = enemyList.ToList();
            foreach (var enemy in enemySnapshot)
            {
                enemy.PlanNextAction();
            }
        }
    }

    public void ExecuteEnemyAction(EnemyTurnStartBattleEvent payload)
    {
        var groupSnapshot = currentEnemies.Values.ToList();

        foreach (var enemyGroup in groupSnapshot)
        {
            var enemySnapshot = enemyGroup.ToList();

            for (int i = enemySnapshot.Count - 1; i >= 0; i--)
            {
                var enemy = enemySnapshot[i];
                
                if (enemy.IsDead) continue;

                var plannedActions = enemy.PlannedActions;

                foreach (var actionData in plannedActions)
                {
                    var executeAction = new ExecuteEnemyActionBattleAction(enemy, actionData);
                    context.ActionScheduler.EnqueueFront(new BattleEntityAction(enemy, executeAction));
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