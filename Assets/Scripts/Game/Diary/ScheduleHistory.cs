using System;
using System.Collections.Generic;

public class ScheduleHistory
{
    private Dictionary<EnemyData, (int encounterCount, int resolvedCount)> encounterEnemies = new Dictionary<EnemyData, (int encounerCount, int resolvedCount)>();
    private Dictionary<IncidentData, int> encounterIncidents = new Dictionary<IncidentData, int>();
    private int transactionCount = 0;

    private Dictionary<BelongingsData, int> equippedBelongingsNodeCounts = new Dictionary<BelongingsData, int>();

    private bool hasMetBoss = false;
    private bool hasResolvedBoss = false;
    private bool hasMentalBroken = false;
    private bool hasEarlyExited = false;
    private int remainMentalityOnExit;

    public ScheduleHistory() {}
    public ScheduleHistory(ScheduleHistorySaveData saveData, EnemyDatabase enemyDatabase, IncidentDatabase incidentDatabase, BelongingsDatabase belongingsDatabase)
    {
        foreach (var pair in saveData.encounterEnemies)
        {
            var data = enemyDatabase.GetData(pair.Key);
            if (data == null) { throw new InvalidOperationException($"[ScheduleHistory] Failed to get enemy data, Id : {pair.Key}"); }

            encounterEnemies[data] = pair.Value;
        }

        foreach (var pair in saveData.encounterIncidents)
        {
            var data = incidentDatabase.GetData(pair.Key);
            if (data == null) { throw new InvalidOperationException($"[ScheduleHistory] Failed to get incident data, Id : {pair.Key}"); }

            encounterIncidents[data] = pair.Value;
        }

        transactionCount = saveData.transactionCount;

        foreach (var pair in saveData.equippedBelongingsNodeCount)
        {
            var entity = belongingsDatabase.GetEntity(pair.Key);
            if (entity == null) { throw new InvalidOperationException($"[ScheduleHistory] Failed to get belongings data, Id : {pair.Key}"); }

            equippedBelongingsNodeCounts[entity.Data] = pair.Value;
        }

        hasMetBoss = saveData.hasMetBoss;
        hasMentalBroken = saveData.hasMentalBroken;
        hasEarlyExited = saveData.hasEarlyExited;
    }

    public IReadOnlyDictionary<EnemyData, (int encounerCount, int resolvedCount)> EncounterEnemies { get => encounterEnemies; }
    public IReadOnlyDictionary<IncidentData, int> EncounterIncidents { get => encounterIncidents; }
    public int TransactionCount { get => transactionCount; }
    public IReadOnlyDictionary<BelongingsData, int> BelongingsEquippingNodeCount { get => equippedBelongingsNodeCounts; }
    public bool HasMetBoss { get => hasMetBoss; }
    public bool HasResolvedBoss { get => hasResolvedBoss; }
    public bool HasMentalBroken { get => hasMentalBroken; set => hasMentalBroken = value; }
    public bool HasEarlyExited { get => hasEarlyExited; set => hasEarlyExited = value; }
    public int RemainMentalityOnExit { get => remainMentalityOnExit; set => remainMentalityOnExit = value; }

    public void RecordEncounterEnemy(EnemyData data, bool isResolved)
    {
        if (!encounterEnemies.ContainsKey(data)) { encounterEnemies[data] = (0, 0); }

        var history = encounterEnemies[data];

        history.encounterCount++;
        history.resolvedCount = isResolved ? history.resolvedCount + 1 : history.resolvedCount;

        encounterEnemies[data] = history;
    }
    public void RecordEncounterBoss(EnemyData enemyData, bool isResolved)
    {  
        if (hasMetBoss) { throw new InvalidOperationException("Already met a boss. Cannot meet boss more than twice in a single schedule."); }

        hasMetBoss = true;
        hasResolvedBoss = isResolved;
        RecordEncounterEnemy(enemyData, isResolved);
    }
    public void RecordEncounterIncident(IncidentData data)
    {
        if (!encounterIncidents.ContainsKey(data)) { encounterIncidents[data] = 0; }

        encounterIncidents[data]++;
    }
    public void RecordTransaction() { transactionCount++; }

    public void RecordEquippedBelongings(BelongingsData data)
    {
        if (!equippedBelongingsNodeCounts.ContainsKey(data)) { equippedBelongingsNodeCounts[data] = 0; }

        equippedBelongingsNodeCounts[data]++;
    }

    internal void RecordEncounterIncident(object data)
    {
        throw new NotImplementedException();
    }
}