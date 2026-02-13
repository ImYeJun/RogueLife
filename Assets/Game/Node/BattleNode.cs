using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;

public class BattleNode : Node
{
    private IEngageBattle battleSystem;
    private List<EnemyDataSlot> engagingEnemiesDataSlot;
    private int startPhaseCount;
    private bool hasResolved;

    public BattleNode(Guid skeletonId, Action<Node, Player> OnMoveRequest, IEngageBattle battleSystem, List<EnemyDataSlot> engagingEnemiesDataSlot) : base(OnMoveRequest, skeletonId)
    {
        this.battleSystem = battleSystem;
        this.engagingEnemiesDataSlot = engagingEnemiesDataSlot;
    }
    public override void OnEnter(Player player, ScheduleHistory scheduleHistory)
    {
        //TODO : engagingEnemiesData에 따라 적 일상 UI 띄우기
        base.OnEnter(player, scheduleHistory);

        //TODO : engagingEnemiesData에 따라 encounterLine 연출 띄우기

        battleSystem.EngageBattle(player, engagingEnemiesDataSlot, startPhaseCount, OnBattleExit);
    }

    public void OnBattleExit(BattleResult result)
    {
        hasResolved = result == BattleResult.PLAYER_WIN;

        switch (result)
        {
            case BattleResult.PLAYER_WIN:
                throw new NotImplementedException(); //TODO 보상 구현
            case BattleResult.ALL_PHASE_END:
                throw new NotImplementedException(); //TODO 패널티 구현
            case BattleResult.PLAYER_DIED:
                OnPlayerMentalBroken();
                return;
            default:
                throw new InvalidOperationException($"{result} is not expected to be used in battle result");
        }
        
        RequestNextNodeSelection();
    }

    protected override void OnExit(Node nextNode)
    {
        foreach (var enemyDataSlot in engagingEnemiesDataSlot)
        {
            var enemyData = enemyDataSlot.Data;

            if (enemyData.Tier == EnemyTier.BOSS) { scheduleHistory.RecordEncounterBoss(enemyData, hasResolved); }
            else { scheduleHistory.RecordEncounterEnemy(enemyData, hasResolved); }
        }
        RecordBelongingsEquipping();

        base.OnExit(nextNode);
    }
}