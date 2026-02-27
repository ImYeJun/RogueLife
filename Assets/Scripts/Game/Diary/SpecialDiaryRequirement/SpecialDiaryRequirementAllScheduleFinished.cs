using System;

[Serializable]
public class SpecialDiaryRequirementAllScheduleFinished : SpecialDiaryRequirement
{
    public override bool IsFulfilled(DiaryContext context)
    {
        return context.AreAllScheduleFinished;
    }
}