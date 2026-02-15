using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChoiceEngageBattleEffect : IChoiceEffect
{
    [SerializeField] private List<EnemyData> engaingEnemyData;
    [SerializeField, Min(0)] private int startPhaseCount;

    public ChoiceEngageBattleEffect() {}

    public void Execute(FieldContext context)
    {
        context.BattleSystem.EngageBattle(engaingEnemyData, startPhaseCount);
    }
}