using System.Collections.Generic;
using UnityEngine;

public class BattleEnemy : BattleEntity
{
    private EnemyData data;
    private List<BattleAction> plannedAction = new List<BattleAction>();
    private int currentHealth;

    public IReadOnlyList<BattleAction> PlannedActions { get => plannedAction; }

    public BattleEnemy(EnemyData enemyData)
    {
        data = enemyData;
        currentHealth = data.MaxBaseHealth;
    }

    public void PlanNextAction()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDead()
    {
        base.OnDead();
    }

    public override void ReceiveDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);

        if (currentHealth <= 0) { OnDead(); }
    }
}