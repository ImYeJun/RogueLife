using System;
using UnityEngine;

[Serializable]
public class ChoiceSetBossEffect : IChoiceEffect
{
    [SerializeField] private EnemyData bossData;

    public ChoiceSetBossEffect(){}

    public void Execute(FieldContext context)
    {
        context.ScheduleSystem.SetBoss(bossData);
    }
}