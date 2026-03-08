using System;
using UnityEngine;

[Serializable]
public class ChoiceSetBossEffect : IChoiceEffect
{
    [SerializeField] private EnemyEntity bossEntity;

    public ChoiceSetBossEffect(){}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.ScheduleSystem.SetBossData(bossEntity);
    }
}