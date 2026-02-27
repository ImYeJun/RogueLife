using System;
using System.Collections.Generic;

public class Diary
{
    private Guid id;
    private DateTime date;
    private Dictionary<int, ScheduleHistory> scheduleHistories;
    private bool areAllSchedulesFinished;
    private FinalEquipment finalEquipment;
    private bool isSpecial;
    private SpecialDiaryData specialDiaryData;

    public Diary(Guid id, DateTime date, Dictionary<int, ScheduleHistory> scheduleHistories, 
                bool areAllSchedulesFinished, FinalEquipment finalEquipment, 
                bool isSpecial, SpecialDiaryData specialDiaryData)
    {
        this.id = id;
        this.date = date;
        this.scheduleHistories = scheduleHistories;
        this.areAllSchedulesFinished = areAllSchedulesFinished;
        this.finalEquipment = finalEquipment;
        this.isSpecial = isSpecial;
        this.specialDiaryData = specialDiaryData;
    }

    public Diary(Guid id, DateTime date, Dictionary<int, ScheduleHistory> scheduleHistories, 
                bool areAllSchedulesFinished, FinalEquipment finalEquipment, SpecialDiaryData specialDiaryData = null)
        : this(id, date, scheduleHistories, areAllSchedulesFinished, finalEquipment, specialDiaryData != null, specialDiaryData) { }

    public Diary(DateTime date, Dictionary<int, ScheduleHistory> scheduleHistories, 
                bool areAllSchedulesFinished, FinalEquipment finalEquipment, SpecialDiaryData specialDiaryData = null)
        : this(Guid.NewGuid(), date, scheduleHistories, areAllSchedulesFinished, finalEquipment, specialDiaryData != null, specialDiaryData) { }

    public Guid Id { get => id; }
    public DateTime Date { get => date; }
    public IReadOnlyDictionary<int, ScheduleHistory> ScheduleHistories { get => scheduleHistories; }
    public bool AreAllSchedulesFinished { get => areAllSchedulesFinished; }
    public FinalEquipment FinalEquipment { get => finalEquipment; }
    public bool IsSpecial { get => isSpecial; }
    public SpecialDiaryData SpecialDiaryData { get => specialDiaryData; }

    public bool Equals(Diary operand)
    {
        return id == operand.Id;
    }
}