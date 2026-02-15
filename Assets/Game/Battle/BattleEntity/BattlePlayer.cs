using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : BattleEntity, IBattleBelongingsOwner
{
    private IBattleHealth playerHealth;
    private List<BattleBelongings> belongings = new List<BattleBelongings>();

    public BattlePlayer(IBattleHealth playerHealth)
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

    public override void ReceiveDamage(int amount) { playerHealth.HurtBattleHealth(amount, false); }
    public void ReceiveMentalDamage(int amount) { playerHealth.HurtMentality(amount); }

    public override void RequestHurt(int amount, HurtSource source)
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
}