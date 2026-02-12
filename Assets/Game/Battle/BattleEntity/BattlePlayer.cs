using UnityEngine;

public class BattlePlayer : BattleEntity
{
    private PlayerHealth playerHealth;

    public BattlePlayer(PlayerHealth playerHealth)
    {
        this.playerHealth = playerHealth;
    }

    public PlayerHealth Health { get => playerHealth; }

    protected override void OnDead()
    {
        base.OnDead();
    }

    public override void ReceiveDamage(int amount)
    {
        playerHealth.HurtBattleHealth(amount, true);
    }

    public override void RequestHurt(int amount, HurtSource source)
    {
        throw new System.NotImplementedException();
    }

    public override void Heal(int amount)
    {
        throw new System.NotImplementedException();
    }
}