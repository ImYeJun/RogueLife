using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleNode : Node
{
    private BattleSystem battleSystem;
    private Player player;
    private List<EnemyData> engagingEnemiesData;
    private int startPhaseCount;

    public BattleNode(Action<Node> OnMoveRequest) : base(OnMoveRequest)
    {
    }

    public override void OnEnter()
    {
        //TODO : engagingEnemiesData에 따라 적 일상 UI 띄우기
        base.OnEnter();

        //TODO : engagingEnemiesData에 따라 encounterLine 연출 띄우기

        battleSystem.OnBattleExit += OnBattleExit;
        battleSystem.EngageBattle(engagingEnemiesData, player, startPhaseCount);
    }

    public void OnBattleExit()
    {
        battleSystem.OnBattleExit -= OnBattleExit;
        RequestNextNodeSelection();
    }
}