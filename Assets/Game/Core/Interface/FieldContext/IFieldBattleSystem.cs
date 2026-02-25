using System;
using System.Collections.Generic;

public interface IFieldBattleSystem : IEngageBattle
{
    public void RegisterBattleStartEffect(BattleStatusEffect buff, FieldEffectDuration duration);
}