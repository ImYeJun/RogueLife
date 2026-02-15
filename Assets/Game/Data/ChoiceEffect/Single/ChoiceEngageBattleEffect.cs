using System;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;

[Serializable]
public class ChoiceEngageBattleEffect : IChoiceEffect
{
    [SerializeField] private List<EnemyData> engaingEnemyData;
    [SerializeField, Min(0)] private int startPhaseCount;

    public ChoiceEngageBattleEffect() {}

    public void Execute(FieldContext context)
    {
        var slots = new List<EnemyDataSlot>();
        foreach (var data in engaingEnemyData)
        {
            slots.Add(new EnemyDataSlot(data));
        }

        // context.BattleSystem.EngageBattle(context.Health, context.ActionCost, context.Deck, context.BelongingsBag, engaingEnemyData, startPhaseCount);
    }
}