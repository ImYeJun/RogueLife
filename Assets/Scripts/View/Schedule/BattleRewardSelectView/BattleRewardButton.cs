using System;
using TMPro;
using UnityEngine;

public class BattleRewardButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private IBattleReward reward;
    private Action onButtonSelected;
    private IScheduleViewCommander commander;

    public void Initiate(IBattleReward reward, Action onButtonSelected, IScheduleViewCommander commander)
    {
        this.reward = reward;
        this.onButtonSelected = onButtonSelected;
        this.commander = commander;

        if (reward is null) { return; }
        text.text = $"{reward.Name} 획득하기";
    }

    public void OnPressed()
    {
        reward?.Resolve(commander);

        onButtonSelected.Invoke();
    }
}