using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewEvent.ScheduleView;

public interface IFieldHealth : IBattleHealth {
    public void HealMentality(int amount, bool isOverflowable);
    public void IncreaseMaxBattleHealth(int amount);
    public void DecreaseMaxBattleHealth(int amount);
    public void IncreaseMaxMentality(int amount);
    public void DecreaseMaxMentality(int amount);

    public event Action<PlayerHurt> OnPlayerHurt;
    public event Action<PlayerHealed> OnPlayerHealed;
}