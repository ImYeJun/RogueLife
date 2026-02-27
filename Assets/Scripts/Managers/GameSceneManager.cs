using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : SingletonManager<GameSceneManager>
{
    private SceneName currentScene;

    public void LoadScene(SceneName name)
    {
        SceneManager.UnloadSceneAsync((int)currentScene);

        SceneManager.LoadScene((int)name, LoadSceneMode.Additive);
    }

    public void StartGameFlow()
    {
        if (SceneManager.loadedSceneCount >= 2) { return; }

        currentScene = SceneName.MAIN_MENU;

        SceneManager.LoadScene((int)SceneName.MAIN_MENU, LoadSceneMode.Additive);
    }

#if UNITY_EDITOR
    public void SetCurrentScene(SceneName name)
    {
        currentScene = name;
    }
#endif
}