using System.Collections.Generic;

public class CompositeBattleReward : IBattleReward
{
    private List<IBattleReward> rewards;
    private string description;

    public CompositeBattleReward(List<IBattleReward> rewards, string description)
    {
        this.rewards = rewards;
        this.description = description;
    }

    public List<IBattleReward> Rewards { get => rewards; }
    public string Description { get => description; }

    public void Resolve(IScheduleViewCommander commander)
    {
        foreach (var reward in rewards)
        {
            reward.Resolve(commander);
        }
    }
}