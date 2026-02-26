using UnityEngine;

namespace UI.StartMenu
{
    public class ExitButton : MonoBehaviour {
        public void OnPressed()
        {
            Application.Quit();
        }
    }
}