using System;
using NUnit.Framework;
using UnityEngine;

public class DiaryStoreTest
{
    [Test]
    public void StoreDiary()
    {
        DiaryArchive diaryArchive = new DiaryArchive();
        Diary diary = new Diary("이것은 테스트 다이어리여", SpecialDiaryImageType.SECOND, DateTime.Now.ToString());

        try
        {
            diaryArchive.AddDiary(diary);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            Assert.Fail();
        }
    }

    [Test]
    public void StoreAndLoadDiary()
    {
        DiaryArchive diaryArchive = new DiaryArchive();
        Diary diary = new Diary("ㅎㅇ", SpecialDiaryImageType.FISRT, DateTime.Now.ToString());

        try
        {
            diaryArchive.AddDiary(diary);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            Assert.Fail("일기 삽입 실패");
        }

        diaryArchive.LoadDiaries();

        Assert.IsTrue(diaryArchive.HasDiary(diary), "일기 가져왔는데, 원본이랑 같은게 없음");
    }
}
