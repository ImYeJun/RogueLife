using System.Collections.Generic;

public interface IStartMenuDiaryCommander {
    public void WatchDiary(Diary diary);
    public List<Diary> GetRecentDiaries(int count = Constant.RECENT_DIARY_COUNT);
    public List<(SpecialDiaryData data, Diary diary)> GetSpecialDiaries();
}
