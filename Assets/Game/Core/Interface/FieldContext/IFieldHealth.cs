
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IFieldHealth {
    public void HealMentality(int amount, bool isOverflowable);
    public void HurtMentality(int amount);
    public void HealBattleHealth(int amount);
    public void HurtBattleHealth(int amount, bool isOverflowable);
    public void IncreaseMaxBattleHealth(int amount);
    public void DecreaseMaxBattleHealth(int amount);
    public void IncreaseMaxMentality(int amount);
    public void DecreaseMaxMentality(int amount);
}