using System.Collections.Generic;

public interface IReadOnlyBattleEntity {
    public IReadOnlyDictionary<BattleStatusEffectData, BattleStatusEffect> CurrentBuffs { get; }
    public IReadOnlyDictionary<BattleStatusEffectData, BattleStatusEffect> CurrentDebuffs { get; }
    public List<BattleStatusEffect> GetBattleStatusEffects(BattleStatusEffectType type = BattleStatusEffectType.ANY);
}