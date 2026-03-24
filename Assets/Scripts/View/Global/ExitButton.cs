using UnityEngine;
using View.Core;
using ViewEvent.StartMenu;

namespace View.StartMenu
{
    public class ExitButton : MonoBehaviour
    {
        public void OnPressed()
        {
            UnityEngine.Debug.Log("게임 종료 버튼 클릭!");
            Application.Quit();
        }
    }
}