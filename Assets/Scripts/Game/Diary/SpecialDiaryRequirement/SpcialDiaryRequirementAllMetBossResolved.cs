using System;

[Serializable]
public class SpcialDiaryRequirementAllMetBossResolved : SpecialDiaryRequirement
{
    public override bool IsFulfilled(DiaryContext context)
    {
        var scheduleHistories = context.ScheduleHistories.Values;

        foreach (var history in scheduleHistories)
        {
            if (!history.HasResolvedBoss) { return false; }
        }

        return true;
    }
}