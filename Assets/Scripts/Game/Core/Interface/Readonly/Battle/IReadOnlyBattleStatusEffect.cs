public interface IReadOnlyBattleStatusEffect {
    public BattleStatusEffectData Data { get; }
    public int StackCount { get; }
    public bool IsDurationEternal { get; }
    public int RemainTurn { get; }
    public bool IsExpired { get; }
    public BattleEntityTrait RequiredTraits { get; }
    public BattleEntityCondition GrantedCondition { get; }
}