using UnityEngine;
using UI.Global;
using ViewEvent.StartMenu;
using View.Core;

namespace View.StartMenu
{
    public class SettingButton : MonoBehaviour {
        public void OnPressed()
        {
            GlobalUIManager.Instance?.OpenSettingUI();
        }
    }
}