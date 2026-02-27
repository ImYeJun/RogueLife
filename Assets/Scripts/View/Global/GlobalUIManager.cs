using System;
using UnityEngine;

namespace UI.Global
{
    public class GlobalUIManager : SingletonManager<GlobalUIManager>
    {
        [SerializeField] private GameObject settingUI;

        public void OpenSettingUI()
        {
            settingUI.SetActive(true);
        }
        public void CloseSettingUI()
        {
            settingUI.SetActive(false);
        }
    }
}
