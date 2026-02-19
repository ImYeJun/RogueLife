using System.Collections.Generic;
using Battle.HurtSources;
using UnityEngine;

public class BattlePlayer : BattleEntity, IBattleBelongingsOwner
{
    private IBattleHealth playerHealth;
    private List<BattleBelongings> belongings = new List<BattleBelongings>();

    public BattlePlayer(BattleContext context, IBattleHealth playerHealth) : base(context, BattleEntityTrait.PLAYER)
    {
        this.playerHealth = playerHealth;
        this.playerHealth.OnMentalBreakDown += OnDead;
    }

    public void SetBelongings(List<BattleBelongings> belongings) { this.belongings = belongings; }
    public List<BattleBelongings> Belongings { get => belongings;  }

    protected override void OnDead()
    {
        base.OnDead();

        playerHealth.OnMentalBreakDown -= OnDead;
        context.BattleScheduler.EndBattle(BattleResult.PLAYER_DIED);
    }

    public void ReceiveDamage(PlayerBattleHurtContext hurtContext, BattleHurtSource source) { 
        if (hurtContext.TotalDamage <= 0) { return; }

        playerHealth.HurtBattleHealth(hurtContext.BattleHealthDamage, false);
        playerHealth.HurtMentality(hurtContext.MentalityDamage);

        context.EventBus.Publish(new EntityHurtBattleEvent(hurtContext.TotalDamage, this, source));
    }

    public override void RequestHurt(int amount, BattleHurtSource source)
    {
        var hurtContext = GenerateHurtContext(amount);

        context.ActionScheduler.Enqueue(new HurtPlayerBattleAction(source, this, hurtContext));
    }
    private PlayerBattleHurtContext GenerateHurtContext(int amount)
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