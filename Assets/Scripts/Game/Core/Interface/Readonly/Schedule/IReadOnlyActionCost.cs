using System;

public interface IReadOnlyActionCost
{
    public int CurrentMaxActionCost { get; }

    public event Action<int> OnMaxActionCostChanged;
}