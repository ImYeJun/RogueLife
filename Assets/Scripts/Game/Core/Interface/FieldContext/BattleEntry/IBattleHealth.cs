using System;

public interface IBattleHealth : IReadOnlyHealth
{
    public event Action OnMentalBreakDown;
    public bool IsFullHealth { get; }
    public void HurtBattleHealth(int amount, bool isOverflowable);
    public void HurtMentality(int amount);
    public void HealBattleHealth(int amount);
    
    public event Action<int, int, bool> OnHurt; 
    public event Action<bool, int, int> OnHealed;
}