using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleNode : Node
{
    private BattleSystem battleSystem;
    private Player player;
    private List<EnemyDataSlot> engagingEnemiesDataSlot;
    private int startPhaseCount;

    public BattleNode(Action<Node> OnMoveRequest, List<EnemyDataSlot> engagingEnemiesDataSlot) : base(OnMoveRequest)
    {
        this.engagingEnemiesDataSlot = engagingEnemiesDataSlot;
    }

    public override void OnEnter()
    {
        //TODO : engagingEnemiesData에 따라 적 일상 UI 띄우기
        base.OnEnter();

        //TODO : engagingEnemiesData에 따라 encounterLine 연출 띄우기

        battleSystem.OnBattleExit += OnBattleExit;
        List<EnemyData> engagingEnemiesData = engagingEnemiesDataSlot.Select(dataSlot => dataSlot.Data).ToList();
        battleSystem.EngageBattle(engagingEnemiesData, player, startPhaseCount);
    }

    public void OnBattleExit(BattleResult result)
    {
        battleSystem.OnBattleExit -= OnBattleExit;
        //TODO : Result에 따른 행동 구현하기
        RequestNextNodeSelection();
    }
}