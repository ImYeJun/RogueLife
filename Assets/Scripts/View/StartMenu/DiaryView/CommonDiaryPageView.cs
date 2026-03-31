using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.StartMenu.DiaryViews
{
    public class CommonDiaryPageView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Button diaryWatchButton; 
        private Diary currentDiary;
        private IStartMenuDiaryCommander commander;

        private void Awake() 
        {
            SetActive(false);
        }

        public void SetDiary(Diary diary)
        {
            currentDiary = diary;

            if (diary == null)
            {
                SetActive(false);
                return;
            }

            title.text = diary.Date.ToString("yyyy년 M월 d일");
            
            diaryWatchButton.onClick.RemoveAllListeners();
            diaryWatchButton.onClick.AddListener(WatchDiary);
            SetActive(true);
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