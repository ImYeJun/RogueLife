using System;
using UnityEngine;
using View.ScheduleView; 

public class BattleRewardButton : SingleTextSelectButton
{
    private IBattleReward reward;
    private IScheduleViewCommander commander;

    public void Initiate(IBattleReward reward, Action onButtonSelected, IScheduleViewCommander commander)
    {
        this.reward = reward;
        this.commander = commander;

        Action action = () => {
            this.reward?.Resolve(this.commander);
            onButtonSelected?.Invoke();
        };

        Initialize(action, reward?.Description);
    }
}