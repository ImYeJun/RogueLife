using System;
using System.Collections.Generic;

public interface IFieldBattleSystem : IEngageBattle
{
    public void RegisterBattleStartBuff(BattleStatusEffect buff, FieldEffectDuration duration);
}