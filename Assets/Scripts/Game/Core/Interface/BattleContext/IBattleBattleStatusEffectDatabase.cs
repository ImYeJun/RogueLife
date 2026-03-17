#nullable enable

using System;

public interface IBattleBattleStatusEffectDatabase
{
    public string GetDescription(string id);
    public BattleStatusEffectEntity? GetRandomData(Random random, BattleStatusEffectType type);
}