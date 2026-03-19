using System;
using System.Collections.Generic;
using Battle.Enemies.Actions;
using Battle.HurtSources;
using UnityEngine;
using ViewEvent.BattleView;

public class BattleEnemy : BattleEntity, IEnemyBehaviourOwner, ICloneableBattleEntity, IReadOnlyBattleEnemy
{    
    private EnemyEntity entity;
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
    public BattleEnemyBehaviour BehaviourInstance { get => behaviourInstance;  }
    public EnemyData Data { get => entity.Data; }
    public bool IsFirstAction => isFirstAction;
    public int PreviousActionCount => previousActionCount;
    public BattleEntity AsEntity => this;
    public BattleHurtSource AsHurtSource => new EntitySource(this);
    public override int MaxHealth => currentMaxHealth;
    public float NormalizedHealth { get => currentMaxHealth == 0 ? 0 : (float)currentHealth/currentMaxHealth; }

    public BattleEnemy(BattleContext context, EnemyEntity enemyEntity) : base(context, BattleEntityTrait.ENEMY)
    {
        entity = enemyEntity;

        var data = entity.Data;
        currentMaxHealth = data.MaxBaseHealth;
        currentHealth = currentMaxHealth;

        behaviourInstance = entity.CloneBehaviour(this);
        isFirstAction = true;
        previousActionCount = 0;
    }

    public void PlanNextAction()
    {
        if (IsDead) { return; }

        plannedActions = behaviourInstance.PlanAction(context);
        previousActionCount = plannedActions.Count;

        viewEventPublisher.Publish(new EnemyActionPlanned(viewEventPublisher.GetNextSequenceId(), this));

        isFirstAction = false;
    }

    protected override void OnDead()
    {
        behaviourInstance.OnOwnerDied(context);
        viewEventPublisher.Publish(new EnemyDied(viewEventPublisher.GetNextSequenceId(), this));
        base.OnDead();
        
        Died?.Invoke(this);
    }

    public void OnSpawned()
    {
        behaviourInstance.OnOwnerSpawned(context);
    }
    
    public override void Heal(int amount)
    {
        if (IsDead) { return; }

        currentHealth = Mathf.Min(currentHealth + amount, currentMaxHealth);
        viewEventPublisher.Publish(new EnemyHealed(viewEventPublisher.GetNextSequenceId(), this, amount, currentHealth));
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
        viewEventPublisher.Publish(new EnemyHurt(viewEventPublisher.GetNextSequenceId(), this, amount, currentHealth));

        if (currentHealth <= 0) { OnDead(); }
    }

    public void Clone(float maxHealthMultiplier = 1.0f)
    {
        var clone = new BattleEnemy(context, entity);
        
        int newMaxHealth = Mathf.Max(1, Mathf.RoundToInt(currentMaxHealth * maxHealthMultiplier));

        clone.currentMaxHealth = newMaxHealth;
        clone.currentHealth = newMaxHealth;

        var spawnEnemyAction = new SpawnEnemyBattleAction(clone);
        context.ActionScheduler.Enqueue(spawnEnemyAction);
    }

    public override BattleHurtSource GetAsHurtSource()
    {
        return new EntitySource(this);
    }
}