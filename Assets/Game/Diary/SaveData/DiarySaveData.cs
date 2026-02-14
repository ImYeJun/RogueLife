using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class DiarySaveData
{
    public string id;
    public string date;
    public Dictionary<int, ScheduleHistorySaveData> scheduleHistories = new Dictionary<int, ScheduleHistorySaveData>();
    public bool areAllSchedulesFinished;
    public FinalEquipmentSaveData finalEquipment;
    public bool isSpecial;
    public string specialDiaryId;

    public DiarySaveData(Diary origin)
    {
        id = origin.Id.ToString();
        date = origin.Date.ToString("o");
        foreach (var pair in origin.ScheduleHistories)
        {
            scheduleHistories[pair.Key] = new ScheduleHistorySaveData(pair.Value);
        }
        areAllSchedulesFinished = origin.AreAllSchedulesFinished;
        finalEquipment = new FinalEquipmentSaveData(origin.FinalEquipment);
        isSpecial = origin.IsSpecial;
        specialDiaryId = isSpecial ? origin.SpecialDiaryData.Id : null;
    }

    [JsonConstructor]
    public DiarySaveData(string id, string date, Dictionary<int, ScheduleHistorySaveData> scheduleHistories, bool areAllSchedulesFinished, FinalEquipmentSaveData finalEquipment, bool isSpecial, string specialDiaryId)
    {
        this.id = id;
        this.date = date;
        this.scheduleHistories = scheduleHistories;
        this.areAllSchedulesFinished = areAllSchedulesFinished;
        this.finalEquipment = finalEquipment;
        this.isSpecial = isSpecial;
        this.specialDiaryId = specialDiaryId;
    }
}