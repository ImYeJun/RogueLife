using System.Collections.Generic;
using Battle.HurtSources;
using UnityEngine;

public class BattlePlayer : BattleEntity, IBattleBelongingsOwner
{
    private IBattleHealth playerHealth;

    public BattlePlayer(BattleContext context, IBattleHealth playerHealth) : base(context, BattleEntityTrait.PLAYER)
    {
        this.playerHealth = playerHealth;
        this.playerHealth.OnMentalBreakDown += OnDead;
    }

    public override bool IsFullHealth => playerHealth.IsFullHealth;
    public override int CurrentHealth => playerHealth.CurrentBattleHealth + playerHealth.CurrentMentality;

    protected override void OnDead()
    {
        base.OnDead();

        playerHealth.OnMentalBreakDown -= OnDead;

        var action = new RequestBattleEndBattleAction(BattleResult.PLAYER_DIED);
        context.ActionScheduler.EnqueueFront(action);
    }

    public void ReceiveDamage(PlayerBattleHurtContext hurtContext, BattleHurtSource source) { 
        if (hurtContext.TotalDamage <= 0) { return; }

        playerHealth.HurtBattleHealth(hurtContext.BattleHealthDamage, false);
        HurtMentality(hurtContext.MentalityDamage);
        // playerHealth.HurtMentality(hurtContext.MentalityDamage);

        context.EventBus.Publish(new EntityHurtBattleEvent(hurtContext.TotalDamage, this, source));
    }

    public void HurtMentality(int damage)
    {
        playerHealth.HurtMentality(damage);
    }

    public override void RequestHurt(int amount, BattleHurtSource source)
    {
        context.ActionScheduler.Enqueue(new RequestHurtPlayerBattleAction(amount, source, this));
    }
    public PlayerBattleHurtContext GenerateHurtContext(int amount)
    {
        int currentBattleHealth = playerHealth.CurrentBattleHealth;

        int battleHealthDamage = Mathf.Min(amount, currentBattleHealth);
        bool isOverflow = amount > battleHealthDamage;
        int mentalityDamage = isOverflow ? amount - battleHealthDamage : 0;
        return new PlayerBattleHurtContext(battleHealthDamage, mentalityDamage, isOverflow);
    }

    public override void Heal(int amount) { playerHealth.HealBattleHealth(amount); }

    public override BattleHurtSource GetAsHurtSource()
    {
        return new EntitySource(this);
    }
}