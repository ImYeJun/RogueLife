using System;
using System.Linq;

[Serializable]
public class SpecialDiaryRequirementEncounterAllIncidents : SpecialDiaryRequirement
{
    public override bool IsFulfilled(DiaryContext context)
    {
        var scheduleHistories = context.ScheduleHistories;
        if (scheduleHistories == null)
        {
            UnityEngine.Debug.LogWarning("[SpecialDiaryRequirementEncounterAllIncidents] there's no schedule histroy.");
            return false;
        }

        var metIncidents = scheduleHistories.Values.SelectMany(sel => sel.EncounterIncidents.Keys).ToHashSet();

        foreach (var incident in context.IncidentDatabase.AvailableIncidents)
        {
            if (!metIncidents.Contains(incident)) { return false; }
        }

        return true;
    }
}