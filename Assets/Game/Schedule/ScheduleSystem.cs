using System;
using UnityEngine;

public class ScheduleSystem
{
    private System.Random random;
    private BattleSystem battleSystem;
    private ScheduleGenerator scheduleGenerator;
    private Action onScheduleEnd;

    private Schedule currentSchedule;
    public Schedule CurrentSchedule { get => currentSchedule; }

    public ScheduleSystem(System.Random random, BattleSystem battleSystem, Action onScheduleEnd)
    {
        this.random = random;
        this.battleSystem = battleSystem;
        this.onScheduleEnd = onScheduleEnd;
    }

    public void StartSchdule()
    {
        
    }

    private void SettleCurrentScheduleData(ScheduleData data)
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
