public interface IScheduleViewCommander : IViewCommander
{
    public void BroadcastCurrentState();
    public void EnterStartNodeIfNeeded();
}