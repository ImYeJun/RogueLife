using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.StartMenu
{
    public class GameStartButton : MonoBehaviour {
        public void OnPressed()
        {
            //TODO RootContoller에게 씬 전환 책임 이양
            GameRunManager.Instance.StartNewRun();
            GameRunManager.Instance.CurrentRun.StartGame();
            GameSceneManager.Instance.LoadScene(SceneName.SCHEDULE_SELECTING);
        }
    }
}