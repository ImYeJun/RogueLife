using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SpecialDiaryRequirementResolveAllEnemies : SpecialDiaryRequirement
{
    [SerializeField] private EnemyTier filteringTier;

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
        
        var originEnemies = context.EnemyDatabase.AvailableEnemies;
        var filteredEnemies = originEnemies.Where(enemy => enemy.Tier == filteringTier);
        foreach (var enemyData in filteredEnemies)
        {
            if (!metEnemy.Contains(enemyData)) { return false; }
        }

        return true;
    }
}