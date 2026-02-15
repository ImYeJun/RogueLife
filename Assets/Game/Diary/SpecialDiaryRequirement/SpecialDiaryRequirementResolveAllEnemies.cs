using System;
using System.Collections.Generic;

[Serializable]
public class SpecialDiaryRequirementResolveAllEnemies : SpecialDiaryRequirement
{
    public override bool IsFulfilled(DiaryContext context)
    {
        var scheduleHistories = context.ScheduleHistories;
        if (scheduleHistories == null)
        {
            UnityEngine.Debug.LogWarning("[SpecialDiaryRequirementResolveAllEnemies] there's no schedule histroy.");
            return false;
        }

        var metEnemy = new HashSet<EnemyData>();

        foreach (var scehduleHistory in scheduleHistories.Values)
        {
            foreach (var encounterEnemy in scehduleHistory.EncounterEnemies)
            {
                metEnemy.Add(encounterEnemy.Key);

                if (encounterEnemy.Value.encounerCount != encounterEnemy.Value.resolvedCount) { return false; }
            }
        }

        foreach (var enemyData in context.EnemyDatabase.AvailableEnemies)
        {
            if (!metEnemy.Contains(enemyData)) { return false; }
        }

        return true;
    }
}