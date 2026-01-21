using System.Collections.Generic;
using UnityEngine;

public class BattleNode : Node
{
    private BattleSystem battleSystem;
    private List<EnemyData> engagingEnemiesData;

    public override void OnEnter()
    {
        //TODO : engagingEnemiesData에 따라 적 일상 UI 띄우기
        base.OnEnter();

        //TODO : engagingEnemiesData에 따라 encounterLine 연출 띄우기

        battleSystem.OnBattleEnd += RequestNextNodeSelection;
        battleSystem.EngageBattle();
    }
}