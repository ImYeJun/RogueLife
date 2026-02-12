using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemy : BattleEntity, IEnemyBehaviourOwner
{    
    private EnemyData data;
    private List<EnemyAction> plannedAction = new List<EnemyAction>();
    private BattleEnemyBehaviour behaviourInstance;
    private int currentHealth;
    private int currentMaxHealth;

    public event Action<BattleEnemy> Died;

    public IReadOnlyList<EnemyAction> PlannedActions { get => plannedAction; }
    public EnemyData Data { get => data; }

    public BattleEnemy(EnemyData enemyData)
    {
        data = enemyData;
        currentMaxHealth = data.MaxBaseHealth;
        currentHealth = currentMaxHealth;
        behaviourInstance = data.CloneBehaviour();
    }

    public void PlanNextAction()
    {
        if (IsDead) { return; }

        throw new System.NotImplementedException();
    }

    protected override void OnDead()
    {
        base.OnDead();

        Died?.Invoke(this);
    }
    
    public override void Heal(int amount)
    {
        if (IsDead) { return; }

        currentHealth = Mathf.Min(currentHealth + amount, currentMaxHealth);
    }

    public override void RequestHurt(int amount, HurtSource source)
    {
        if (IsDead) { return; }
        
        context.ActionScheduler.Enqueue(new HurtEnemyBattleAction(this, source, amount));
    }

    public override void ReceiveDamage(int amount)
    {
        if (IsDead) { return; }

        currentHealth = Mathf.Max(currentHealth - amount, 0);

        if (currentHealth <= 0) { OnDead(); }
    }
}