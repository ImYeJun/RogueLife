using System;

public class RunDiarySystem
{
    private DiaryContext context;
    private SpecialDiaryDatabase specialDiaryDatabase;
    private IRunDiaryEnemyDatabaseContext enemyDatabase;
    private IRunDiaryIncidentDatabaseContext incidentDatabase;
    private DiaryArchive archive = new DiaryArchive();

    public RunDiarySystem(SpecialDiaryDatabase specialDiaryDatabase)
    {
        context = new DiaryContext();
        this.specialDiaryDatabase = specialDiaryDatabase;
    }

    public void RecordScheduleHistory(int index, ScheduleHistory history)
    {
        context.RecordScheduleHistory(index, history);
    }

    public void WriteDiary(IRunDiaryPlayerDeck playerDeck, IRunDiaryPlayerBelongingsBag playerBelongingsBag, bool areAllScheduleFinished)
    {
        context.Date = DateTime.Now;
        
        SpecialDiaryData speicalDiaryData;
        context.RecordFinalEquipments(playerDeck, playerBelongingsBag);
        context.AreAllScheduleFinished = areAllScheduleFinished;

        Diary diary;
        if (specialDiaryDatabase.TryGetSpecialDiaryData(context, out speicalDiaryData))
        {
            diary = new Diary(speicalDiaryData.Description, SpecialDiaryImageType.FISRT, DateTime.Now.ToString());
        }
        else
        {
            diary = new Diary("TEST", SpecialDiaryImageType.NONE, DateTime.Now.ToString());
        }

        archive.AddDiary(diary);
    }
}