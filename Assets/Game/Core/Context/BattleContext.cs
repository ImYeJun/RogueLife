using System;
using UnityEngine;

public class BattleContext
{
    private BattleEntity actor;
    private BattleEntity target;
    private bool isBattleEnd = false;
    private bool hasSpecialVictoryExecuted = false;

    public bool IsBattleEnd { get => isBattleEnd; }
    public bool HasSpecialVictoryExecuted { get => hasSpecialVictoryExecuted; }

    public void AttackTarget(int amount)
    {
        target.ReceiveDamage(amount);
    }

    public void ReqeustSpecialVictory()
    {
        hasSpecialVictoryExecuted = true;
        RequestBattleEnd();
    }

    public void RequestBattleEnd()
    {
        isBattleEnd = true;
    }
}