using System;
using UnityEngine;

[Serializable]
public class ChoiceObtainBattleStartBuffEffect : IChoiceEffect
{
    [SerializeField] private BattleStatusEffect buff;
    [SerializeField] private FieldEffectDuration duration;

    public ChoiceObtainBattleStartBuffEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        context.BattleSystem.RegisterBattleStartEffect(buff, duration);
    }
}