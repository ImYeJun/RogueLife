using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleNode : Node
{
    private IEngageBattle battleSystem;
    private List<EnemyDataSlot> engagingEnemiesDataSlot;
    private int startPhaseCount;

    public BattleNode(Guid skeletonId, IEngageBattle battleSystem, Action<Node, Player> OnMoveRequest, List<EnemyDataSlot> engagingEnemiesDataSlot) : base(OnMoveRequest, skeletonId)
    {
        this.battleSystem = battleSystem;
        this.engagingEnemiesDataSlot = engagingEnemiesDataSlot;
    }

    public override void OnEnter(Player player)
    {
        //TODO : engagingEnemiesData에 따라 적 일상 UI 띄우기
        base.OnEnter(player);

        //TODO : engagingEnemiesData에 따라 encounterLine 연출 띄우기

        // battleSystem.OnBattleExit += OnBattleExit;
        battleSystem.EngageBattle(player, engagingEnemiesDataSlot, startPhaseCount, OnBattleExit);
    }

    public void OnBattleExit(BattleResult result)
    {
        // battleSystem.OnBattleExit -= OnBattleExit;
        //TODO : Result에 따른 행동 구현하기
        RequestNextNodeSelection();
    }
}