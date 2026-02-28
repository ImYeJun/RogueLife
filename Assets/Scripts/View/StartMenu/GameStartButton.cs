using UnityEngine;
using UnityEngine.SceneManagement;
using View.Core;
using ViewEvent.StartMenu;

namespace View.StartMenu
{
    public class GameStartButton : InteractableViewBehaviour<IStartMenuViewEvent, IStartMenuViewCommander>
    {
        public override void OnDestroy()
        {
        }

        public override void OnInitialized()
        {
        }

        public void OnPressed()
        {            
            commander.RequestStartDeckSelect();
        }
    }
}