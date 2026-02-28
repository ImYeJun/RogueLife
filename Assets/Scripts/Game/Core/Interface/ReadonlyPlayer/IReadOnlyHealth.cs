using System;

public interface IReadOnlyHealth
{
    public int CurrentBattleHealth { get; }
    public int CurrentMentality { get; }
    public int MaxBattleHealth { get; }
    public int MaxMentality { get; }
    public float NormalizedBattleHealth { get; }
    public float NomarlizedMentality { get; }

    public event Action<int> OnBattleHealthChanged;
    public event Action<int> OnMentalityChanged;
}