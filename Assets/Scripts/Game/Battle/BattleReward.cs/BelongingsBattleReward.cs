public class BelongingsBattleReward : IBattleReward
{
    private Belongings belongings;

    public BelongingsBattleReward(Belongings belongings)
    {
        this.belongings = belongings;
    }

    public string Name => $"{belongings.Name} (소지품)";

    public void Resolve(IScheduleViewCommander commander)
    {
        commander.ObtainBelongings(belongings);
    }
}