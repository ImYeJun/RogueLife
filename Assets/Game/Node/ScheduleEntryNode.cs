using System;
using UnityEngine;

class ScheduleEntryNode : Node
{
    public event Action<ScheduleData> OnScheduleSettled;

    public override void OnEnter()
    {
        base.OnEnter();
        //TODO : 일정 테마 선택 기능 구현
    }

    public void SelectSchedule(ScheduleData selectedScheduleData)
    {
        OnScheduleSettled.Invoke(selectedScheduleData);
        RequestNextNodeSelection();
    }
}