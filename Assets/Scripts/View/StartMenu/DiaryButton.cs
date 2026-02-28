using UnityEngine;
using View.Core;
using ViewEvent.StartMenu;
namespace View.StartMenu
{
    public class DiaryButton : InteractableViewBehaviour<IStartMenuViewEvent, IStartMenuViewCommander>
    {
        public override void OnDestroy()
        {
            
        }

        public override void OnInitialized()
        {
            
        }

        public void OnPressed()
        {
            UnityEngine.Debug.Log("일기 버튼 클릭!");
        }
    }
}