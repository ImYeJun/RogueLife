using System;
using ViewEvent.WriteDiaryView;

public class RunDiarySystem : IWriteDiaryViewCommander
{
    private SequenceIdGenerator sequenceIdGenerator = new SequenceIdGenerator();
    private WriteDiaryViewEventBus viewEventBus = new WriteDiaryViewEventBus();
    private DiaryContext context;
    private SpecialDiaryDatabase specialDiaryDatabase;
    private DiaryArchive archive;

    private bool isPending;
    public Action OnRunEnded;

    private IRunDiaryPlayerDeck pendingDeck;
    private IRunDiaryPlayerBelongingsBag pendingBelongingsBag;
    private bool pendingAreAllScheduleFinished;

    public RunDiarySystem(SpecialDiaryDatabase specialDiaryDatabase, ScheduleDatabase scheduleDatabase, EnemyDatabase enemyDatabase, IncidentDatabase incidentDatabase, BelongingsDatabase belongingsDatabase, CardDatabase cardDatabase)
    {
        context = new DiaryContext(enemyDatabase, incidentDatabase);
        archive = new DiaryArchive(enemyDatabase, incidentDatabase, belongingsDatabase, cardDatabase, specialDiaryDatabase, scheduleDatabase);
        this.specialDiaryDatabase = specialDiaryDatabase;
    }
    
    public WriteDiaryViewEventBus ViewEventBus => viewEventBus;

    public void RecordScheduleHistory(int index, ScheduleHistory history)
    {
        context.RecordScheduleHistory(index, history);
    }

    public void PendDiary(Action onRunEnded, IRunDiaryPlayerDeck playerDeck, IRunDiaryPlayerBelongingsBag playerBelongingsBag, bool areAllScheduleFinished)
    {
        OnRunEnded = onRunEnded;
        pendingDeck = playerDeck;
        pendingBelongingsBag = playerBelongingsBag;
        pendingAreAllScheduleFinished = areAllScheduleFinished;
        isPending = true;
    }

    public void WriteDiary()
    {
        if (!isPending)
        {
            throw new InvalidOperationException("[RunDiarySystem/WriteDiary] There is no pending diary data to write. Please call PendDiary first.");
        }

        context.Date = DateTime.Now;
        
        context.RecordFinalEquipments(pendingDeck, pendingBelongingsBag);
        context.AreAllScheduleFinished = pendingAreAllScheduleFinished;

        SpecialDiaryData specialDiaryData;
        Diary diary;
        if (specialDiaryDatabase.TryGetData(context, out specialDiaryData))
        {
            diary = new Diary(context.Date, context.ScheduleHistories, context.AreAllScheduleFinished, context.FinalEquipment, specialDiaryData);
        }
        else
        {
            diary = new Diary(context.Date, context.ScheduleHistories, context.AreAllScheduleFinished, context.FinalEquipment);
        }

        archive.AddDiary(diary);
        viewEventBus.Publish(new DiaryWritten(sequenceIdGenerator.GetNextId(), diary));
        
        isPending = false;
        pendingDeck = null;
        pendingBelongingsBag = null;
    }

    public void RequestReturnToMainMenu()
    {
        OnRunEnded.Invoke();
        viewEventBus.Publish(new ReturnToMainMenuRequested(sequenceIdGenerator.GetNextId()));
    }
}