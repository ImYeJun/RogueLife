using System;
using UnityEngine;

namespace UI.Global
{
    public class GlobalUIManager : SingletonManager<GlobalUIManager>
    {
        [SerializeField] private GameObject settingUI;
        [SerializeField] private GameObject gotoMainMenuButton;
        [SerializeField] private GameObject warningText;

        public void OpenSettingUI()
        {
            CheckGameRun();
            settingUI.SetActive(true);
        }
        public void CloseSettingUI()
        {
            settingUI.SetActive(false);
        }

        private void CheckGameRun()
        {
            bool isGameRunning = GameRunManager.Instance.CurrentRun is not null;
            SetInGameIndicatorActive(isGameRunning);
        }

        public void SetInGameIndicatorActive(bool isGameRunning)
        {
            gotoMainMenuButton.SetActive(isGameRunning);
            warningText.SetActive(isGameRunning);
        }
    }
}
