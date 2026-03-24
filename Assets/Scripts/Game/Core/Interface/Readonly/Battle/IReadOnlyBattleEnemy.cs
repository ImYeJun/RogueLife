using System.Collections.Generic;
using Battle.Enemies.Actions;

public interface IReadOnlyBattleEnemy : IReadOnlyBattleEntity{
    public IReadOnlyList<EnemyAction> PlannedActions { get; }
    public EnemyData Data { get; }
    public int CurrentHealth { get; }
    public int MaxHealth { get; }
    public float NormalizedHealth { get; }
    public IReadOnlyDictionary<string, EnemyAction> AvailableActions { get; }
}