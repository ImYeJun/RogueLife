using System;
using UnityEngine;

[Serializable]
public class ChoiceObtainBattleStartBuffEffect : IChoiceEffect
{
    [SerializeField] private BattleStatusEffect buff;
    [SerializeField] private FieldEffectDuration duration;

    public ChoiceObtainBattleStartBuffEffect() {}

    public void Execute(FieldContext context)
    {
        context.BattleSystem.RegisterBattleStartBuff(buff, duration);
    }
}