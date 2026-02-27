using System;
using System.Collections.Generic;
using Battle.StartEffects;

public interface IFieldBattleSystem : IEngageBattle
{
    public void AddBattleStartEffect(BattleStartEffect effect);
}