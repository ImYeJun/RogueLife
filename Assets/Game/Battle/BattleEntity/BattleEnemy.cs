using System;
using System.Collections.Generic;
using Battle.HurtSources;
using UnityEngine;

public class BattleEnemy : BattleEntity, IEnemyBehaviourOwner, ICloneableBattleEntity
{    
    private EnemyData data;
    private List<EnemyAction> plannedActions = new List<EnemyAction>();
    private BattleEnemyBehaviour behaviourInstance;
    private bool isFirstAction;
    private int previousActionCount;
    private int currentHealth;
    private int currentMaxHealth;

    public event Action<BattleEnemy> Died;

    public override int CurrentHealth => currentHealth;
    public override bool IsFullHealth => currentHealth >= currentMaxHealth;
    public IReadOnlyList<EnemyAction> PlannedActions { get => plannedActions; }
    public EnemyData Data { get => data; }
    public bool IsFirstAction => isFirstAction;
    public int PreviousActionCount => previousActionCount;


    public BattleEnemy(BattleContext context, EnemyData enemyData) : base(context, BattleEntityTrait.ENEMY)
    {
        data = enemyData;
        currentMaxHealth = data.MaxBaseHealth;
        currentHealth = currentMaxHealth;

        behaviourInstance = data.CloneBehaviour(this);
        isFirstAction = true;
        previousActionCount = 0;
    }

    public void PlanNextAction()
    {
        if (IsDead) { return; }

        plannedActions = behaviourInstance.PlanAction(context.Random);
        previousActionCount = plannedActions.Count;
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

    public override void RequestHurt(int amount, BattleHurtSource source)
    {
        if (IsDead) { return; }
        
        context.ActionScheduler.Enqueue(new HurtEnemyBattleAction(this, source, amount));
    }

    public void ReceiveDamage(int amount, BattleHurtSource source)
    {
        if (IsDead) { return; }
        if (amount <= 0) { return; }

        int determinedAmount = Mathf.Min(amount, currentHealth);
        currentHealth -= determinedAmount;

        context.EventBus.Publish(new EntityHurtBattleEvent(determinedAmount, this, source));
        if (currentHealth <= 0) { OnDead(); }
    }

    public void Clone(float maxHealthMultiplier = 1.0f)
    {
        var clone = new BattleEnemy(context, data);
        
        int newMaxHealth = Mathf.Max(1, Mathf.RoundToInt(clone.currentMaxHealth * maxHealthMultiplier));

        clone.currentMaxHealth = newMaxHealth;
        clone.currentHealth = newMaxHealth;

        context.EnemySystem.SpawnEnemy(clone);
    }

    public override BattleHurtSource GetAsHurtSource()
    {
        return new EntitySource(this);
    }
}