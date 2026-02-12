using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleEntity : IBattleStatusEffectOwner
{
    protected BattleContext context;
    private List<BattleStatusEffect> equippedBuffs = new List<BattleStatusEffect>();
    private List<BattleStatusEffect> equippedDebuffs = new List<BattleStatusEffect>();

    public IReadOnlyList<BattleStatusEffect> CurrentBuffs { get => equippedBuffs; }
    public IReadOnlyList<BattleStatusEffect> CurrentDebuffs { get => equippedDebuffs; }

    protected bool isDead = false;

    public bool IsDead { get => isDead; }

    public abstract void ReceiveDamage(int amount);
    public abstract void RequestHurt(int amount, HurtSource source);
    public abstract void Heal(int amount);
    public void RequestHeal(int amount)
    {
        context.ActionScheduler.Enqueue(new HealEntityBattleAction(this, amount));
    }

    protected virtual void OnDead()
    {
        isDead = true;

        var expiredBuffs = equippedBuffs.ToArray();
        var expiredDebuffs = equippedDebuffs.ToArray();

        equippedBuffs.Clear();
        equippedDebuffs.Clear();

        foreach (var buff in expiredBuffs) { buff.OnRemoved(); }
        foreach (var debuff in expiredDebuffs) { debuff.OnRemoved(); }
    }

    public void RequestApplyBuff(BattleStatusEffect buff)
    {
        if (IsDead) { return; }
        
        context.ActionScheduler.Enqueue(new ApplyEntityBuffBattleAction(this, buff));
    }
    public void ApplyBuff(BattleStatusEffect buff)
    {
        if (IsDead) { return; }
        
        if (equippedBuffs.Contains(buff)) { 
            UnityEngine.Debug.LogWarning("The Entity is already equipping given buff.");
            return;
        }

        equippedBuffs.Add(buff);
        buff.OnApplied();
    }
    public void ApplyDebuff(BattleStatusEffect debuff)
    {
        if (IsDead) { return; }
        
        if (equippedBuffs.Contains(debuff)) { 
            UnityEngine.Debug.LogWarning("The Entity is already equipping given debuff.");
            return;
        }

        equippedDebuffs.Add(debuff);
        debuff.OnApplied();
    }

    public void RequestRemoveStatusEffect(BattleStatusEffect statusEffect)
    {
        if (IsDead) { return; }
        
        context.ActionScheduler.Enqueue(new RemoveEntityStatusEffect(this, statusEffect));
    }
    public void RemoveStatusEffect(BattleStatusEffect statusEffect)
    {
        if (IsDead) { return; }
        
        if (equippedBuffs.Contains(statusEffect)) { RemoveBuff(statusEffect); }
        else if (equippedDebuffs.Contains(statusEffect)) { RemoveDebuff(statusEffect); }
        else { throw new InvalidOperationException("The battle entity doesn't contain given status effect"); }
    }
    public void RemoveBuff(BattleStatusEffect buff)
    {
        if (IsDead) { return; }
        
        if (!equippedBuffs.Contains(buff))
        {
            UnityEngine.Debug.LogWarning("The Entity isn't equipping given buff.");
            return;
        }

        buff.OnRemoved();
        equippedBuffs.Remove(buff);
    }
    public void RemoveDebuff(BattleStatusEffect debuff)
    {
        if (IsDead) { return; }
        
        if (!equippedDebuffs.Contains(debuff))
        {
            UnityEngine.Debug.LogWarning("The Entity isn't equipping given debuff.");
            return;
        }
        
        debuff.OnRemoved();
        equippedBuffs.Remove(debuff);
    }
}