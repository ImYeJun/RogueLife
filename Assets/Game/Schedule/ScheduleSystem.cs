using System;
using UnityEngine;

public class ScheduleSystem
{
    private System.Random random;
    private ScheduleGenerator scheduleGenerator;
    private Action onScheduleEnd;

    private Schedule currentSchedule;
    public Schedule CurrentSchedule { get => currentSchedule; }

    public ScheduleSystem(System.Random random, IEngageBattle battleSystem, Action onScheduleEnd)
    {
        this.random = random;
        this.onScheduleEnd = onScheduleEnd;

        // scheduleGenerator= new ScheduleGenerator()
    }

    public void StartSchdule()
    {
        
    }

    public void SettleCurrentScheduleData(ScheduleData data)
    {
        currentSchedule = scheduleGenerator.GenerateSchedule(random, data);
        currentSchedule.OnEnd += EndSchedule;
    }

    public void EndSchedule()
    {
        onScheduleEnd?.Invoke();
        currentSchedule.OnEnd -= EndSchedule;
    }
}
