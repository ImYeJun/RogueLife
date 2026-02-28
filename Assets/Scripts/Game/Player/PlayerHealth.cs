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
    public event Action<int> OnBattleHealthChanged;
    public event Action<int> OnMentalityChanged;

    public bool IsFullHealth => currentBattleHealth >= maxBattleHealth && currentMentality >= maxMentality;
    public int CurrentBattleHealth { get => currentBattleHealth; }
    public int CurrentMentality { get => currentMentality; }
    public int MaxBattleHealth { get => maxBattleHealth; }
    public int MaxMentality { get => maxMentality; }


    public void HurtBattleHealth(int amount, bool isOverflowable)
    {
        if (amount < 0) return;

        int actualDamage = amount;
        int overflowAmount = 0;

        if (currentBattleHealth < amount)
        {
            overflowAmount = amount - currentBattleHealth;
            actualDamage = currentBattleHealth; 
        }

        currentBattleHealth -= actualDamage; 
        
        OnMentalityChanged?.Invoke(currentBattleHealth);

        if (overflowAmount > 0 && isOverflowable)
        {
            HurtMentality(overflowAmount);
        }
    }

    public void HurtMentality(int amount)
    {
        if (amount < 0) return;

        bool wasBroken = IsMentalBrokenDown();

        currentMentality = Mathf.Max(0, currentMentality - amount);
        OnMentalityChanged?.Invoke(currentMentality);

        if (!wasBroken && IsMentalBrokenDown())
        {
            OnMentalBreakDown?.Invoke();
        }
    }

    public bool IsMentalBrokenDown() => currentMentality <= 0;

    public void HealBattleHealth(int amount)
    {
        if (amount < 0) return;
        currentBattleHealth = Mathf.Min(currentBattleHealth + amount, maxBattleHealth);

        OnMentalityChanged?.Invoke(currentBattleHealth);
    }

    public void HealMentality(int amount, bool isOverflowable)
    {
        if (amount < 0) return;

        int overflowAmount = 0;
        
        if (currentMentality + amount > maxMentality)
        {
            overflowAmount = (currentMentality + amount) - maxMentality;
            amount -= overflowAmount; 
        }

        currentMentality += amount;
        OnMentalityChanged?.Invoke(currentMentality);

        if (overflowAmount > 0 && isOverflowable)
        {
            HealBattleHealth(overflowAmount);
        }
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