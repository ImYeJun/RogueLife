using System;

public interface IBattleHealth
{
    public event Action OnMentalBreakDown;
    public int CurrentBattleHealth { get; }
    public int CurrentMentality { get; }
    public bool IsFullHealth { get; }
    public void HurtBattleHealth(int amount, bool isOverflowable);
    public void HurtMentality(int amount);
    public void HealBattleHealth(int amount);
}