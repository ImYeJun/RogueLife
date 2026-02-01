using System;

public class RunDiarySystem
{
    private DiaryContext context;
    private SpecialDiaryDatabase specialDiaryDatabase;
    private DiaryArchive archive = new DiaryArchive();

    public RunDiarySystem(SpecialDiaryDatabase specialDiaryDatabase)
    {
        this.specialDiaryDatabase = specialDiaryDatabase;
    }

    public void WriteDiary()
    {
        SpeicalDiaryData speicalDiaryData;

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