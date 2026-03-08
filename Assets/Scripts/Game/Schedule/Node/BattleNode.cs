using System;
using System.Collections.Generic;
using System.Linq;
using Battle.BattleResultCommands;
using NUnit.Framework.Constraints;
using UnityEngine;

public class BattleNode : Node
{
    private IEngageBattle battleSystem;
    private List<EnemyDataSlot> engagingEnemiesDataSlot;
    private bool hasResolved;

    public BattleNode(Guid skeletonId, IEngageBattle battleSystem, List<EnemyDataSlot> engagingEnemiesDataSlot) : base(skeletonId)
    {
        this.battleSystem = battleSystem;
        this.engagingEnemiesDataSlot = engagingEnemiesDataSlot;
    }

    public bool IsBossNode => engagingEnemiesDataSlot.Any(slot => slot.Entity.Tier == EnemyTier.BOSS);

    public override void OnEnter(FieldContext context, IScheduleRouter flowHandler, ScheduleHistory scheduleHistory)
    {
        base.OnEnter(context, flowHandler, scheduleHistory);

        RequestNextNodeSelection();
        return;
        
        battleSystem.EngageBattle(context.Health, context.ActionCost, context.Deck, context.BelongingsBag, engagingEnemiesDataSlot, OnBattleExit);
    }

    public void OnBattleExit(BattleResultCommand resultCommand)
    {
        context.Health.OnMentalBreakDown += OnPlayerMentalBroken;
        
        resultCommand.Resolve(context, this);
    }

    public override void OnExit(Node nextNode)
    {
        foreach (var enemyDataSlot in engagingEnemiesDataSlot)
        {
            var enemyEntity = enemyDataSlot.Entity;

            if (enemyEntity.Tier == EnemyTier.BOSS) { scheduleHistory.RecordEncounterBoss(enemyEntity.Data, hasResolved); }
            else { scheduleHistory.RecordEncounterEnemy(enemyEntity.Data, hasResolved); }
        }
        RecordBelongingsEquipping();

        context.Health.OnMentalBreakDown -= OnPlayerMentalBroken;
        base.OnExit(nextNode);
    }
}