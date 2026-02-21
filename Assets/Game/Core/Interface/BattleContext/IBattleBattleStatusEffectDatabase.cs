#nullable enable

using System;

public interface IBattleBattleStatusEffectDatabase
{
    public BattleStatusEffectData? GetRandomData(Random random, BattleStatusEffectType type);
}