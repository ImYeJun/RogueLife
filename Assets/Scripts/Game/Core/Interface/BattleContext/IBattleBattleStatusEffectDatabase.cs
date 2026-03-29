#nullable enable

using System;

public interface IBattleBattleStatusEffectDatabase
{
    public BattleStatusEffectEntity? GetRandomData(Random random, BattleStatusEffectType type);
    public BattleStatusEffectEntity? GetRandomData(Random random, BattleStatusEffectType type, BattleEntityTrait trait);
    BattleStatusEffectData GetData(string id);
}