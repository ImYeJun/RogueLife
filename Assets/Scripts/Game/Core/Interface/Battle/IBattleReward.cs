public interface IBattleReward {
    public string Description { get; }
    public void Resolve(IScheduleViewCommander commander);
}