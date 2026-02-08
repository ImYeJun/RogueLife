using UnityEngine;

public class PlayerActionCost : IFieldActionCost
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

    public void IncreaseMaxCapacity(int amount) {
        if (amount < 0) return;

        maxActionCost += amount;
    }
    public void DecreaseMaxCapacity(int amount) {
        if (amount < 0) return;

        maxActionCost = Mathf.Max(maxActionCost - amount, 0);
        currentActionCost = Mathf.Min(currentActionCost, maxActionCost);
    }

    public void IncreaseMaxCapacity(int amount, FieldEffectDuration duration)
    {
        throw new System.NotImplementedException();
    }

    public void DecreaseMaxCapacity(int amount, FieldEffectDuration duration)
    {
        throw new System.NotImplementedException();
    }
}