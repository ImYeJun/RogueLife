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

    public BattleNode(Guid skeletonId, Action<Node, FieldContext> OnMoveRequest, IEngageBattle battleSystem, List<EnemyDataSlot> engagingEnemiesDataSlot) : base(OnMoveRequest, skeletonId)
    {
        this.battleSystem = battleSystem;
        this.engagingEnemiesDataSlot = engagingEnemiesDataSlot;
    }
    public override void OnEnter(FieldContext context, ScheduleHistory scheduleHistory)
    {
        //TODO : engagingEnemiesData에 따라 적 일상 UI 띄우기
        base.OnEnter(context, scheduleHistory);

        //TODO : engagingEnemiesData에 따라 encounterLine 연출 띄우기

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
            var enemyData = enemyDataSlot.Data;

            if (enemyData.Tier == EnemyTier.BOSS) { scheduleHistory.RecordEncounterBoss(enemyData, hasResolved); }
            else { scheduleHistory.RecordEncounterEnemy(enemyData, hasResolved); }
        }
        RecordBelongingsEquipping();

        context.Health.OnMentalBreakDown -= OnPlayerMentalBroken;
        base.OnExit(nextNode);
    }
}