using UnityEngine;
using UI.Global;
using ViewEvent.StartMenu;
using View.Core;

namespace View.StartMenu
{
    public class SettingButton : InteractableViewBehaviour<IStartMenuViewEvent, IStartMenuViewCommander> {
        public override void OnDestroy()
        {
        }

        public override void OnInitialized()
        {
        }

        public void OnPressed()
        {
            GlobalUIManager.Instance?.OpenSettingUI();
        }
    }
}