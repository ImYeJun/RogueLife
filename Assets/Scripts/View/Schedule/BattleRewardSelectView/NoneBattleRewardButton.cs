using System;
using UnityEngine;
using View.ScheduleView; 

public class NoneBattleRewardButton : SingleTextSelectButton
{
    public void Initiate(Action onButtonSelected)
    {
        Action action = () => {
            onButtonSelected?.Invoke();
        };

        Initialize(action, "보상 받지 않기");
    }
}