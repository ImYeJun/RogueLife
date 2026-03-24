using System.Collections.Generic;

public class BattleRewardCollector {
    private List<IBattleReward> rewardCandidates = new List<IBattleReward>();

    public List<IBattleReward> RewardCandidates { get => rewardCandidates; }

    public void AddCandidate(IBattleReward candidate)
    {
        rewardCandidates.Add(candidate);
    }
}