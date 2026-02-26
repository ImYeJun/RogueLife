using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : SingletonManager<GameSceneManager>
{
    public void LoadScene(SceneName name)
    {
        SceneManager.LoadScene((int)name);
    }

    public void StartGameFlow()
    {
        SceneManager.LoadScene((int)SceneName.MAIN_MENU, LoadSceneMode.Additive);
    }
}