#nullable enable

using System;

public interface IBattleBattleStatusEffectDatabase
{
    public BattleStatusEffectEntity? GetRandomData(Random random, BattleStatusEffectType type);
}