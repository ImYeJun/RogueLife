using System;
using UnityEngine;

[Serializable]
public class ObtainBattleStartBuff : IChoiceEffect
{
    [SerializeField] private BattleStatusEffect buff;
    [SerializeField] private FieldEffectDuration duration;

    public ObtainBattleStartBuff() {}

    public void Execute(FieldContext context)
    {
        context.BattleSystem.RegisterBattleStartBuff(buff, duration);
    }
}