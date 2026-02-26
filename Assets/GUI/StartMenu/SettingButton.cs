using UnityEngine;
using UI.Global;

namespace UI.StartMenu
{
    public class SettingButton : MonoBehaviour {
        public void OnPressed()
        {
            GlobalUIManager.Instance?.OpenSettingUI();
        }
    }
}