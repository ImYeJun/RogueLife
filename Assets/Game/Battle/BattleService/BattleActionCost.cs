using System;

public class BattleAcionCost : IBattleActionCost, IBattleEventObserver
{
    private int currentActionCost;
    private int maxActionCost;

    private BattleActionCostHistory history;

    public int ActionCost => currentActionCost;

    public bool HasEnough(int amount)
    {
        throw new NotImplementedException();
    }

    public void Consume(int amount)
    {
        throw new NotImplementedException();
    }

    public void Restore(int amount)
    {
        throw new NotImplementedException();
    }

    public void OnBattleEvent(BattleEvent battleEvent)
    {
        throw new NotImplementedException();
    }
}