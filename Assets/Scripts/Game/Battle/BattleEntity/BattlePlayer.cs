using System.Collections.Generic;
using Battle.HurtSources;
using UnityEngine;
using ViewEvent.BattleView;

public class BattlePlayer : BattleEntity, IBattleBelongingsOwner, IReadOnlyBattlePlayer
{
    private IBattleHealth playerHealth;

    public BattlePlayer(BattleContext context, IBattleHealth playerHealth) : base(context, BattleEntityTrait.PLAYER)
    {
        this.playerHealth = playerHealth;
        this.playerHealth.OnMentalBreakDown += OnDead;
    }

    public override bool IsFullHealth => playerHealth.IsFullHealth;
    public override int CurrentHealth => playerHealth.CurrentBattleHealth + playerHealth.CurrentMentality;

    public override int MaxHealth => playerHealth.MaxBattleHealth + playerHealth.MaxMentality;

    public IReadOnlyHealth Health => playerHealth;

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
        playerHealth.HurtMentality(hurtContext.MentalityDamage);

        context.EventBus.Publish(new EntityHurtBattleEvent(hurtContext.TotalDamage, this, source));
        viewEventPublisher.Publish(new PlayerHurt(viewEventPublisher.GetNextSequenceId(), this, hurtContext.BattleHealthDamage, hurtContext.MentalityDamage, playerHealth.CurrentBattleHealth, playerHealth.CurrentMentality, hurtContext.IsOverflow));
    }

    public void HurtMentality(int damage)
    {
        playerHealth.HurtMentality(damage);
        viewEventPublisher.Publish(new PlayerHurt(viewEventPublisher.GetNextSequenceId(), this, 0, damage, playerHealth.CurrentBattleHealth, playerHealth.CurrentMentality, false));
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

    public override void Heal(int amount) { 
        playerHealth.HealBattleHealth(amount);
        viewEventPublisher.Publish(new PlayerHealed(viewEventPublisher.GetNextSequenceId(), this, amount, playerHealth.CurrentBattleHealth));
    }

    public override BattleHurtSource GetAsHurtSource()
    {
        return new EntitySource(this);
    }
}