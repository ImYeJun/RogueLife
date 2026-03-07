using System;
using UnityEngine;

public class PlayerHealth : IFieldHealth
{
    private int currentBattleHealth;
    private int currentMentality;
    private int maxBattleHealth;
    private int maxMentality;

    public PlayerHealth()
    {
        maxMentality = Constant.INITIAL_MAX_MENTALITY;
        currentMentality = maxMentality;

        maxBattleHealth = Constant.INITIAL_MAX_BATTLE_HEALTH;
        currentBattleHealth = maxBattleHealth;
    }

    public event Action OnMentalBreakDown;
    
    public event Action<int, int, bool> OnHurt; 
    public event Action<bool, int, int> OnHealed;

    public bool IsFullHealth => currentBattleHealth >= maxBattleHealth && currentMentality >= maxMentality;
    public int CurrentBattleHealth { get => currentBattleHealth; }
    public int CurrentMentality { get => currentMentality; }
    public int MaxBattleHealth { get => maxBattleHealth; }
    public int MaxMentality { get => maxMentality; }
    public float NormalizedBattleHealth => maxBattleHealth == 0 ? 0 : (float)currentBattleHealth/maxBattleHealth;
    public float NomarlizedMentality => maxMentality == 0 ? 0 : (float)currentMentality/maxMentality;

    public void HurtBattleHealth(int amount, bool isOverflowable)
    {
        if (amount <= 0) return;

        int actualDamage = amount;
        int overflowAmount = 0;

        if (currentBattleHealth < amount)
        {
            overflowAmount = amount - currentBattleHealth;
            actualDamage = currentBattleHealth; 
        }

        currentBattleHealth -= actualDamage; 
        
        int actualMentalityDamage = 0;
        bool isOverflowed = overflowAmount > 0 && isOverflowable;

        if (isOverflowed)
        {
            actualMentalityDamage = ProcessMentalityDamage(overflowAmount);
        }

        OnHurt?.Invoke(actualDamage, actualMentalityDamage, isOverflowed);
    }

    public void HurtMentality(int amount)
    {
        if (amount <= 0) return;

        int actualDamage = ProcessMentalityDamage(amount);

        OnHurt?.Invoke(0, actualDamage, false);
    }
    
    private int ProcessMentalityDamage(int amount)
    {
        bool wasBroken = IsMentalBrokenDown();
        
        int actualDamage = Mathf.Min(currentMentality, amount);

        currentMentality = Mathf.Max(0, currentMentality - amount);

        if (!wasBroken && IsMentalBrokenDown())
        {
            OnMentalBreakDown?.Invoke();
        }

        return actualDamage;
    }

    public bool IsMentalBrokenDown() => currentMentality <= 0;


    private int ProcessBattleHealthHeal(int amount)
    {
        int actualHeal = Mathf.Min(amount, maxBattleHealth - currentBattleHealth);
        currentBattleHealth += actualHeal;
        return actualHeal;
    }

    public void HealBattleHealth(int amount)
    {
        if (amount <= 0) return;
        
        int actualHeal = ProcessBattleHealthHeal(amount);
        
        OnHealed?.Invoke(false, actualHeal, 0);
    }

    public void HealMentality(int amount, bool isOverflowable)
    {
        if (amount <= 0) return;

        int overflowAmount = 0;
        int actualMentalityHeal = amount;
        
        if (currentMentality + amount > maxMentality)
        {
            overflowAmount = (currentMentality + amount) - maxMentality;
            actualMentalityHeal = maxMentality - currentMentality;
        }

        currentMentality += actualMentalityHeal;

        int actualBattleHealthHeal = 0;
        bool isOverflowed = overflowAmount > 0 && isOverflowable;

        if (isOverflowed)
        {
            actualBattleHealthHeal = ProcessBattleHealthHeal(overflowAmount);
        }

        OnHealed?.Invoke(isOverflowed, actualBattleHealthHeal, actualMentalityHeal);
    }

    public void IncreaseMaxBattleHealth(int amount)
    {
        if (amount < 0) return;
        maxBattleHealth += amount;
    }

    public void DecreaseMaxBattleHealth(int amount)
    {
        if (amount < 0) return;
        maxBattleHealth = Mathf.Max(0, maxBattleHealth - amount);
        currentBattleHealth = Mathf.Min(currentBattleHealth, maxBattleHealth);
    }

    public void IncreaseMaxMentality(int amount)
    {
        if (amount < 0) return;
        maxMentality += amount;
    }

    public void DecreaseMaxMentality(int amount)
    {
        if (amount < 0) return;

        bool wasBroken = IsMentalBrokenDown();

        maxMentality = Mathf.Max(0, maxMentality - amount);
        currentMentality = Mathf.Min(currentMentality, maxMentality);

        if (!wasBroken && IsMentalBrokenDown())
        {
            OnMentalBreakDown?.Invoke();
        }
    }
}