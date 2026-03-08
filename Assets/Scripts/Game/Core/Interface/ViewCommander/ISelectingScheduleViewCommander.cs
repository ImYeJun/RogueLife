using UnityEngine;

public interface ISelectingScheduleViewCommander : IViewCommander
{
    public void SettleCurrentScheduleData(ScheduleData data, Vector2 selectPos);
}