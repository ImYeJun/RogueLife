using System;

public class RunDiarySystem
{
    private DiaryContext context;
    private SpecialDiaryDatabase specialDiaryDatabase;
    private DiaryArchive archive;

    public RunDiarySystem(SpecialDiaryDatabase specialDiaryDatabase, EnemyDatabase enemyDatabase, IncidentDatabase incidentDatabase, BelongingsDatabase belongingsDatabase, CardDatabase cardDatabase)
    {
        context = new DiaryContext(enemyDatabase, incidentDatabase);
        archive = new DiaryArchive(enemyDatabase, incidentDatabase, belongingsDatabase, cardDatabase, specialDiaryDatabase);
        this.specialDiaryDatabase = specialDiaryDatabase;
    }

    public void RecordScheduleHistory(int index, ScheduleHistory history)
    {
        context.RecordScheduleHistory(index, history);
    }

    public void WriteDiary(IRunDiaryPlayerDeck playerDeck, IRunDiaryPlayerBelongingsBag playerBelongingsBag, bool areAllScheduleFinished)
    {
        context.Date = DateTime.Now;
        
        context.RecordFinalEquipments(playerDeck, playerBelongingsBag);
        context.AreAllScheduleFinished = areAllScheduleFinished;

        SpecialDiaryData specialDiaryData;
        Diary diary;
        if (specialDiaryDatabase.TryGetData(context, out specialDiaryData))
        {
            diary = new Diary(context.Date, context.ScheduleHistories, context.AreAllScheduleFinished, context.FinalEquipment);
        }
        else
        {
            diary = new Diary(context.Date, context.ScheduleHistories, context.AreAllScheduleFinished, context.FinalEquipment, specialDiaryData);
        }

        archive.AddDiary(diary);
    }
}