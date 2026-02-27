using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.StartMenu
{
    public class GameStartButton : MonoBehaviour {
        public void OnPressed()
        {
            GameRunManager.Instance.StartNewRun();
            GameSceneManager.Instance?.LoadScene(SceneName.SCHEDULE_SELECTING);
        }
    }
}