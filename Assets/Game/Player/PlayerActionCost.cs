using UnityEngine;

public class PlayerActionCost
{
    private int maxActionCost = Constant.BASE_MAX_ACTION_COST;
    private int currentActionCost;

    public int MaxActionCost { get => maxActionCost; }
    public int CurrentActionCost { get => currentActionCost; }
    
    public bool TrySpend(int amount)
    {
        if (amount < 0) { return false; }
        if (currentActionCost < amount) { return false; }

        currentActionCost -= amount;
        return true;
    }

    public void Refill() { currentActionCost = maxActionCost; }

    public void IncreaseMaxActionCost(int amount) {
        if (amount < 0) return;

        maxActionCost += amount;
    }
    public void DecreaseMaxActionCost(int amount) {
        if (amount < 0) return;

        maxActionCost = Mathf.Max(maxActionCost - amount, 0);
        currentActionCost = Mathf.Min(currentActionCost, maxActionCost);
    }
}