using System;
using UnityEngine;

[Serializable]
public class CardEnemyResolveReward : EnemyResolveReward {
    [SerializeField] private CardRarity lowestRarity;
    [SerializeField] private CardRarity highestRarity;
    [SerializeField] private int amount;

    public CardEnemyResolveReward(CardRarity lowestRarity, CardRarity highestRarity, int amount = 1)
    {
        this.lowestRarity = lowestRarity;
        this.highestRarity = highestRarity;
        this.amount = amount;
    }

    public CardRarity LowestRarity { get => lowestRarity; }
    public CardRarity HighestRarity { get => highestRarity; }
    public int Amount { get => amount; }
}