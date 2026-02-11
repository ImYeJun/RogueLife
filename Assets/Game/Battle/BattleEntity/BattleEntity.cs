using UnityEngine;

public abstract class BattleEntity
{
    protected bool isDead = false;

    public bool IsDead { get => isDead; }

    public abstract void ReceiveDamage(int amount);
    public virtual void OnDead()
    {
        isDead = true;
    }

    public void ApplyBuff()
    {
        
    }
    public void ApplyDebuff()
    {
        
    }
}