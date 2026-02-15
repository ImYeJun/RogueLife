using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class SpecialDiaryRequirementsFinalEquippingBelongings : SpecialDiaryRequirement
{
    [SerializeField] BelongingsData data;

    public override bool IsFulfilled(DiaryContext context)
    {
        var finalMainBag = context.FinalEquipment.FinalMainBelongings;
        if (finalMainBag == null)
        {
            UnityEngine.Debug.LogWarning("[SpecialDiaryRequirementsFinalEquippingBelongings] there's no finalMainBag.");
            return false;
        }

        return finalMainBag.Contains(data);
    }
}