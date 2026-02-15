using System;
using UnityEngine;

[Serializable]
public class SpecialDiaryRequirementBelongingsEquipNodeCount : SpecialDiaryRequirement
{
    [SerializeField] BelongingsData data;
    [SerializeField] int requiredNodeCount;

    public override bool IsFulfilled(DiaryContext context)
    {
        var belongingsEquipNodeCount = 0;
        var scheduleHistories = context.ScheduleHistories;
        if (scheduleHistories == null)
        {
            UnityEngine.Debug.LogWarning("[SpecialDiaryRequirementBelongingsEquipNodeCount] there's no schedule histroy.");
            return false;
        }

        foreach (var scheduleHistory in scheduleHistories.Values)
        {
            var belongingsEquipHistory = scheduleHistory.BelongingsEquippingNodeCount;
            if (belongingsEquipHistory.TryGetValue(data, out int amount))
            {
                belongingsEquipNodeCount += amount;
            }
        }

        return belongingsEquipNodeCount >= requiredNodeCount;
    }
}