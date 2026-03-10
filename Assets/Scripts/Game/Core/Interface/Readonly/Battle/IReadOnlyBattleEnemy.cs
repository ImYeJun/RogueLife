public interface IReadOnlyBattleEnemy : IReadOnlyBattleEntity{
    public EnemyData Data { get; }
    public int CurrentHealth { get; }
    public int MaxHealth { get; }
    public float NormalizedHealth { get; }
}