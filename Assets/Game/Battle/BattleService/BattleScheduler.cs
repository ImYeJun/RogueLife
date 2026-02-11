using System;

public class BattleScheduler : IBattleScheduler
{
    private bool isBattleEnd;
    private Action<BattleResult> onBattleEnd;

    public void StartPhase()
    {
        
    }

    public void StartPlayerTurn()
    {
        
    }

    public void EndPlayerTurn()
    {
        
    }

    public void StartEnemyTurn()
    {
        
    }

    public void EndEnemyTurn()
    {
        
    }

    public void EndPhase()
    {
        
    }

    public void EndBattle(BattleResult result)
    {
        
        onBattleEnd?.Invoke(result);
    }
}