using System;
using UnityEngine;

[Serializable]
public class SpecialDiaryRequirementRemainMentality : SpecialDiaryRequirement
{
    [SerializeField] private int amount;

    public override bool IsFulfilled(DiaryContext context)
    {
        var lastScheduleHistroy = context.LastScheduleHistory;
        
        if (lastScheduleHistroy == null) { 
            UnityEngine.Debug.LogWarning("[SpecialDiaryRequirementRemainMentality] there's no schedule histroy.");
            return false;
        }

        return lastScheduleHistroy.RemainMentalityOnExit >= amount;
    }
}