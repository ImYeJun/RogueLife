using System;
using System.Collections.Generic;
using System.Linq;

public class DiaryContext
{
    private IRunDiaryEnemyDatabaseContext enemyDatabase;
    private IRunDiaryIncidentDatabaseContext incidentDatabase;
    private DateTime date;
    private Dictionary<int, ScheduleHistory> scheduleHistories = new Dictionary<int, ScheduleHistory>();
    private bool areAllScheduleFinished;
    private FinalEquipment finalEquipment;

    public DiaryContext(IRunDiaryEnemyDatabaseContext enemyDatabase, IRunDiaryIncidentDatabaseContext incidentDatabase)
    {
        this.enemyDatabase = enemyDatabase;
        this.incidentDatabase = incidentDatabase;
    }

    public IRunDiaryEnemyDatabaseContext EnemyDatabase { get => enemyDatabase; }
    public IRunDiaryIncidentDatabaseContext IncidentDatabase { get => incidentDatabase; }
    public DateTime Date { get => date; set => date = value; }
    public Dictionary<int, ScheduleHistory> ScheduleHistories { get => scheduleHistories; }
    public ScheduleHistory LastScheduleHistory {
        get
        {
            if (scheduleHistories.Count == 0) { return null; }

            var lastIndex = scheduleHistories.Keys.OrderByDescending(sel => sel).First();
            return scheduleHistories[lastIndex];
        }
    }
    public bool AreAllScheduleFinished { get => areAllScheduleFinished; set => areAllScheduleFinished = value; }
    public FinalEquipment FinalEquipment { get => finalEquipment; }

    public void RecordScheduleHistory(int index, ScheduleHistory history)
    {
        scheduleHistories[index] = history;
    }

    public void RecordFinalEquipments(IRunDiaryPlayerDeck playerDeck, IRunDiaryPlayerBelongingsBag playerBelongingsBag)
    {
        finalEquipment = new FinalEquipment(
            finalMainDeck : playerDeck.GetClonedMainDeck(),
            finalSideDeck : playerDeck.GetClonedSideDeck(),
            finalMainBelongings : playerBelongingsBag.GetClonedMainBag(),
            finalSideBelongings : playerBelongingsBag.GetClonedSideBag()
        );
    }
}