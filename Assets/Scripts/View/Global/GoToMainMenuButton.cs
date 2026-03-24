using UnityEngine;

public class GoToMainMenuButton : MonoBehaviour {
    public void OnPressed()
    {
        PresentationManager.Instance.KillAllPresentation();
        GameSceneManager.Instance.LoadScene(SceneName.MAIN_MENU);
        GameRunManager.Instance.OnRunEnded();
    }
}