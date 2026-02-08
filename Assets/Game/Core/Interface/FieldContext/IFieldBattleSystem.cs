using System;
using System.Collections.Generic;

public interface IFieldBattleSystem
{
    public void EngageBattle(List<EnemyData> engagingEnemiesData, int startPahseCount);
    public void RegisterBattleStartBuff(BattleStatusEffect buff, FieldEffectDuration duration);
}