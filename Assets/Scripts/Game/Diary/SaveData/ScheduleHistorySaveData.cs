using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class ScheduleHistorySaveData
{
    public string scheduleDataId;
    public Dictionary<string, (int encounterCount, int resvoledCount)> encounterEnemies = new Dictionary<string, (int encounterCount, int resvoledCount)>();
    public Dictionary<string, int> encounterIncidents = new Dictionary<string, int>();
    public int transactionCount;
    public Dictionary<string, int> equippedBelongingsNodeCount = new Dictionary<string, int>();
    public bool hasMetBoss;
    public bool hasMentalBroken;
    public bool hasEarlyExited;

    public ScheduleHistorySaveData(ScheduleHistory origin)
    {
        scheduleDataId = origin.Data.Id;
        foreach (var pair in origin.EncounterEnemies)
        {
            encounterEnemies[pair.Key.Id] = pair.Value;
        }

        foreach (var pair in origin.EncounterIncidents)
        {
            encounterIncidents[pair.Key.Id] = pair.Value;
        }

        transactionCount = origin.TransactionCount;

        foreach (var pair in origin.BelongingsEquippingNodeCount)
        {
            equippedBelongingsNodeCount[pair.Key.Id] = pair.Value;
        }

        hasMetBoss = origin.HasMetBoss;
        hasMentalBroken = origin.HasMentalBroken;
        hasEarlyExited = origin.HasEarlyExited;
    }

    [JsonConstructor]
    public ScheduleHistorySaveData(string scheduleDataId, Dictionary<string, (int encounterCount, int resvoledCount)> encounterEnemies, Dictionary<string, int> encounterIncidents, int transactionCount, Dictionary<string, int> equippedBelongingsNodeCount, bool hasMetBoss, bool hasMentalBroken, bool hasEarlyExited)
    {
        this.scheduleDataId = scheduleDataId;
        this.encounterEnemies = encounterEnemies;
        this.encounterIncidents = encounterIncidents;
        this.transactionCount = transactionCount;
        this.equippedBelongingsNodeCount = equippedBelongingsNodeCount;
        this.hasMetBoss = hasMetBoss;
        this.hasMentalBroken = hasMentalBroken;
        this.hasEarlyExited = hasEarlyExited;
    }
}