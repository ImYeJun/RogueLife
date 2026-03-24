public interface IBattleReward {
    public string Name { get; }
    public void Resolve(IScheduleViewCommander commander);
}