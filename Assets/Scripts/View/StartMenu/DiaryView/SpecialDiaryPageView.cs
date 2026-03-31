#nullable enable

using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.StartMenu.DiaryViews
{
    public class SpecialDiaryPageView : MonoBehaviour 
    {
        [SerializeField] private GameObject requirementsIndicators; 
        [SerializeField] private TextMeshProUGUI requirementsText; 
        [SerializeField] private Button diaryWatchButton;
        
        private IStartMenuDiaryCommander commander;
        private Diary? currentDiary;

        private void Awake() 
        {
            SetActive(false);
        }

        public void SetDiary(SpecialDiaryData? data, Diary? diary)
        {
            currentDiary = diary;

            if (data == null)
            {
                SetActive(false);
                return;
            }

            SetActive(true);

            if (diary == null)
            {
                StringBuilder sb = new StringBuilder();
                
                for (int i = 0; i < data.Requirements.Count; i++)
                {
                    sb.AppendLine($"{i + 1} : {data.Requirements[i]}"); 
                }

                requirementsText.text = sb.ToString();
                
                requirementsIndicators.SetActive(true);
                diaryWatchButton.gameObject.SetActive(false);
            }
            else
            {
                diaryWatchButton.onClick.RemoveAllListeners();
                diaryWatchButton.onClick.AddListener(WatchDiary);
                
                requirementsIndicators.SetActive(false);
                diaryWatchButton.gameObject.SetActive(true);
            }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void WatchDiary()
        {
            if (currentDiary == null || commander == null) return;

            commander.WatchDiary(currentDiary);
        }

        public void SetCommander(IStartMenuDiaryCommander commander)
        {
            this.commander = commander;
        }
    }
}