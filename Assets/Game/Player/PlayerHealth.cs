using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth
{
    private int currentBattleHealth;
    private int currentMentality;
    private int maxBattleHealth;
    private int maxMentality;
    public event Action OnMentalBreakDown;

    public int CurrentBattleHealth { get => currentBattleHealth; }
    public int CurrentMentality { get => currentMentality; }
    public int MaxBattleHealth { get => maxBattleHealth; }
    public int MaxMentality { get => maxMentality; }

    public void ReceiveDamage(int amount)
    {
        if (currentBattleHealth > amount)
        {
            currentBattleHealth -= amount;
        }
        else
        {
            amount -= currentBattleHealth;
            currentBattleHealth = 0;

            currentMentality = Mathf.Clamp(currentMentality - amount, 0, maxMentality);

            if (IsMentalBrokenDown())
            {
                OnMentalBreakDown?.Invoke();
            }
        }
    }

    public void HealBattleHealth(int amount) { 
        if (amount < 0) return;
        
        currentBattleHealth = Mathf.Clamp(currentBattleHealth + amount, 0, maxBattleHealth);
    }
    public void HealMentality(int amount) {
        if (amount < 0) return;
        
        currentMentality = Mathf.Clamp(currentMentality + amount, 0, maxMentality);
    }
    public void IncreaseMaxBattleHealth(int amount) {
        if (amount < 0) return;
        
        maxBattleHealth += amount;
    }
    public void DecreaseMaxBattleHealth(int amount) {
        if (amount < 0) return;
        
        maxBattleHealth = Mathf.Max(maxBattleHealth - amount, 0);
        currentBattleHealth = Mathf.Min(currentBattleHealth, maxBattleHealth);
    }
    public void IncreaseMaxMentality(int amount) {
        if (amount < 0) return;
        
        maxMentality += amount;
    }
    public void DecreaseMaxMentality(int amount) {
        if (amount < 0) return;
        
        maxMentality = Mathf.Max(maxMentality - amount, 0);
        currentMentality = Mathf.Min(currentMentality, maxMentality);
    }
    public bool IsMentalBrokenDown() => currentMentality <= 0;
}