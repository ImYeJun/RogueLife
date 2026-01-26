
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IChoiceHealth {
    public void HealBattleHealth(int amount);
    public void HealMentality(int amount);
    public void IncreaseMaxBattleHealth(int amount);
    public void DecreaseMaxBattleHealth(int amount);
    public void IncreaseMaxMentality(int amount);
    public void DecreaseMaxMentality(int amount);
    public void ReceiveDamage(int amount);
    public void HurtMentality(int amount);

}