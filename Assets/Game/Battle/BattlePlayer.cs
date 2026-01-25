using UnityEngine;

public class BattlePlayer : BattleEntity
{
    private PlayerHealth playerHealth;

    public BattlePlayer(PlayerHealth playerHealth)
    {
        this.playerHealth = playerHealth;
    }

    public PlayerHealth Health { get => playerHealth; }

    public override void OnDead()
    {
        base.OnDead();
    }

    public override void ReceiveDamage(int amount)
    {
        throw new System.NotImplementedException();
    }
}