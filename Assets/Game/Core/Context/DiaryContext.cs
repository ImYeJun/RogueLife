using System;
using System.Collections.Generic;

public class DiaryContext
{
    private DateTime date;
    private Dictionary<int, ScheduleHistory> scheduleHistories = new Dictionary<int, ScheduleHistory>();
    private bool areAllScheduleFinished;
    private FinalEquipment finalEquipments;

    public DateTime Date { get => date; set => date = value; }
    public Dictionary<int, ScheduleHistory> ScheduleHistories { get => scheduleHistories; }
    public bool AreAllScheduleFinished { get => areAllScheduleFinished; set => areAllScheduleFinished = value; }
    public FinalEquipment FinalEquipments { get => finalEquipments; }

    public void RecordScheduleHistory(int index, ScheduleHistory history)
    {
        scheduleHistories[index] = history;
    }

    public void RecordFinalEquipments(IRunDiaryPlayerDeck playerDeck, IRunDiaryPlayerBelongingsBag playerBelongingsBag)
    {
        finalEquipments = new FinalEquipment(
            finalMainDeck : playerDeck.GetClonedMainDeck(),
            finalSideDeck : playerDeck.GetClonedSideDeck(),
            finalMainBelongings : playerBelongingsBag.GetClonedMainBag(),
            finalSideBelongings : playerBelongingsBag.GetClonedSideBag()
        );
    }
}