using System;
using System.Collections.Generic;

public class ScheduleHistory
{
    private Dictionary<EnemyData, (int ecounerCount, int resolvedCount)> encounterEnemies = new Dictionary<EnemyData, (int ecounerCount, int resolvedCount)>();
    private Dictionary<IncidentData, int> encounterIncidents = new Dictionary<IncidentData, int>();
    private int transactionCount = 0;

    private Dictionary<BelongingsData, int> equippedBelongingsCounts = new Dictionary<BelongingsData, int>();

    private bool hasMetBoss = false;
    private bool hasResolvedBoss = false;
    private bool hasMentalBroken = false;
    private bool hasEarlyExited = false;
    private int remainMentalityOnExit;

    public IReadOnlyDictionary<EnemyData, (int ecounerCount, int resolvedCount)> EncounterEnemies { get => encounterEnemies; }
    public IReadOnlyDictionary<IncidentData, int> EncounterIncidents { get => encounterIncidents; }
    public int TransactionCount { get => transactionCount; }
    public IReadOnlyDictionary<BelongingsData, int> BelongingsEquippingNodeCount { get => equippedBelongingsCounts; }
    public bool HasResolvedBoss { get => hasResolvedBoss; }
    public bool HasMentalBroken { get => hasMentalBroken; set => hasMentalBroken = value; }
    public bool HasEarlyExited { get => hasEarlyExited; set => hasEarlyExited = value; }
    public int RemainMentalityOnExit { get => remainMentalityOnExit; set => remainMentalityOnExit = value; }

    public void RecordEncounterEnemy(EnemyData data, bool isResolved)
    {
        if (!encounterEnemies.ContainsKey(data)) { encounterEnemies[data] = (0, 0); }

        var history = encounterEnemies[data];

        history.ecounerCount++;
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
        if (!equippedBelongingsCounts.ContainsKey(data)) { equippedBelongingsCounts[data] = 0; }

        equippedBelongingsCounts[data]++;
    }
}