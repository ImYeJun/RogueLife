using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class DiaryArchive
{
    private List<Diary> diaries = new List<Diary>();
    private ScheduleDatabase scheduleDatabase;
    private EnemyDatabase enemyDatabase;
    private IncidentDatabase incidentDatabase;
    private BelongingsDatabase belongingsDatabase;
    private CardDatabase cardDatabase;
    private SpecialDiaryDatabase specialDiaryDatabase;

    public DiaryArchive(EnemyDatabase enemyDatabase, IncidentDatabase incidentDatabase, BelongingsDatabase belongingsDatabase, CardDatabase cardDatabase, SpecialDiaryDatabase specialDiaryDatabase, ScheduleDatabase scheduleDatabase)
    {
        this.enemyDatabase = enemyDatabase;
        this.incidentDatabase = incidentDatabase;
        this.belongingsDatabase = belongingsDatabase;
        this.cardDatabase = cardDatabase;
        this.specialDiaryDatabase = specialDiaryDatabase;
        this.scheduleDatabase = scheduleDatabase;
    }

    public void AddDiary(Diary diary)
    {
        DiarySaveData saveData = new DiarySaveData(diary);

        string json = JsonConvert.SerializeObject(saveData);
        // string json = JsonUtility.ToJson(diary);
        string encryptedJson = json;
        // string encryptedJson = EncryptDecrypt(json);

        string diaryID = Guid.NewGuid().ToString();
        string savePath = Path.Combine(Constant.DIARY_STORE_PATH, $"{diaryID}.sav");

        EnsureDiaryDirectoryExists();
        File.WriteAllText(savePath, encryptedJson);
    }

    public void LoadDiaries()
    {
        EnsureDiaryDirectoryExists();
        string[] diaryPaths = Directory.GetFiles(Constant.DIARY_STORE_PATH, "*.sav");

        diaries.Clear();

        foreach (string path in diaryPaths)
        {
            try 
            {
                string encryptedJson = File.ReadAllText(path);
                string json = encryptedJson; 
                // string json = EncryptDecrypt(encryptedJson);

                DiarySaveData saveData = JsonConvert.DeserializeObject<DiarySaveData>(json);
                
                if (saveData == null) { throw new Exception("SaveData is null"); }

                Diary diary = MaterializeSaveData(saveData);
                diaries.Add(diary);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"[DiaryArchive] Failed to load diary file: {path}\nError: {e.Message}");
            }
        }
    }

    private Diary MaterializeSaveData(DiarySaveData saveData)
    {
        Guid id = new Guid(saveData.id);
        
        DateTime date;
        if (!DateTime.TryParse(saveData.date, out date))
        {
            throw new InvalidOperationException("[DiaryArchive] The given date format is not valid.");
        }

        Dictionary<int, ScheduleHistory> scheduleHistories = new Dictionary<int, ScheduleHistory>();
        foreach(var pair in saveData.scheduleHistories)
        {
            scheduleHistories[pair.Key] = new ScheduleHistory(pair.Value, enemyDatabase, incidentDatabase, belongingsDatabase, scheduleDatabase);
        }

        bool areAllScheduleFinished = saveData.areAllSchedulesFinished;

        FinalEquipment finalEquipment = new FinalEquipment(saveData.finalEquipment, cardDatabase, belongingsDatabase);

        bool isSpecial = saveData.isSpecial;
        SpecialDiaryData specialDiaryData = null;
        if (isSpecial) { 
            specialDiaryData = specialDiaryDatabase.GetData(saveData.specialDiaryId);
            if (specialDiaryData == null) { throw new InvalidOperationException($"[DiaryArchive] Failed to get special diary data, Id : {saveData.specialDiaryId}");}
        }

        return isSpecial ? 
            new Diary(id, date, scheduleHistories, areAllScheduleFinished, finalEquipment, specialDiaryData) :
            new Diary(id, date, scheduleHistories, areAllScheduleFinished, finalEquipment);
    }

    public List<Diary> GetRecentDiaries(int count = 5)
    {
        return diaries.OrderBy(diary => diary.Date).Take(count).ToList();
    }

    public bool TryGetSpecialDiary(SpecialDiaryData data, out Diary diary)
    {
        var findedDiary = diaries.FirstOrDefault(d => d.SpecialDiaryData == data);

        if (findedDiary == null)
        {
            diary = null;
            return false;
        }
        else
        {
            diary = findedDiary;
            return true;
        }
    }

    public bool HasDiary(Diary operand)
    {
        return diaries.Any(diary => diary.Equals(operand));
    }

    private string EncryptDecrypt(string data)
    {
        char[] buffer = new char[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            buffer[i] = (char)(data[i] ^ Constant.ENCODE_KEY[i % Constant.ENCODE_KEY.Length]);
        }

        return new string(buffer);
    }

    private void EnsureDiaryDirectoryExists()
    {
        if (!Directory.Exists(Constant.DIARY_STORE_PATH)) { Directory.CreateDirectory(Constant.DIARY_STORE_PATH); }
    }
}