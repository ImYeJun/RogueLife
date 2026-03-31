using UnityEngine;


//TODO : Refactor not to use Singleton
//This class is only for showing the StartMenu's selected diary in the WriteDiary scene.
public class StartMenuDiaryPender : SingletonManager<StartMenuDiaryPender>
{
    public Diary pendingDiary;
}
