using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using View.StartMenu;
using ViewEvent.StartMenu;

public class StartMenuManager : MonoBehaviour, IStartMenuViewCommander
{
    private SequenceIdGenerator sequenceIdGenerator = new SequenceIdGenerator();

    [SerializeField] private List<StartDeck> startDecks;
    [SerializeField] private AudioData bgm;
    private DiaryArchive diaryArchive;
    
    private StartMenuViewEventBus viewEventBus;

    public StartMenuViewEventBus ViewEventBus { get => viewEventBus; }

    public void FixStartDeck(StartDeck startDeck)
    {
        if (StartMenuDiaryPender.Instance is not null)
        {
            StartMenuDiaryPender.Instance.pendingDiary = null;  
        }
        
        GameRunManager.Instance.StartNewRun(startDeck);

        viewEventBus.Publish(new ReadyToStartGame(sequenceIdGenerator.GetNextId()));
        SoundManager.Instance?.StopBgm();
    }

    public void RequestStartDeckSelect()
    {
        viewEventBus.Publish(new StartDeckLoaded(sequenceIdGenerator.GetNextId(), startDecks));
    }

    public List<Diary> GetRecentDiaries(int count = Constant.RECENT_DIARY_COUNT)
    {
        return diaryArchive.GetRecentDiaries(count);
    }

    public void Initialize()
    {
        var databases = DatabaseManager.Instance.Databaes;
        diaryArchive = new DiaryArchive(databases.enemyDatabase, databases.incidentDatabase, databases.belongingsDatabase, databases.cardDatabase, databases.specialDiaryDatabase, databases.scheduleDatabase);
        diaryArchive.LoadDiaries();

        viewEventBus = new StartMenuViewEventBus();
        SoundManager.Instance?.PlayeBgm(bgm);
    }

    public void WatchDiary(Diary diary)
    {
        if (StartMenuDiaryPender.Instance is not null)
        {
            StartMenuDiaryPender.Instance.pendingDiary = diary;  
            
            SoundManager.Instance?.StopBgm();
            GameSceneManager.Instance.LoadScene(SceneName.WRITE_DIARY);
        }
    }

    public List<(SpecialDiaryData data, Diary diary)> GetSpecialDiaries()
    {
        var availableData = DatabaseManager.Instance.Databaes.specialDiaryDatabase.AvailableData;

        return availableData.Select(data => {
            diaryArchive.TryGetSpecialDiary(data, out Diary diary);
            return (data, diary);
            }).ToList();
    }
}
