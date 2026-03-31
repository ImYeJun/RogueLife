using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View.StartMenu.DiaryViews
{
    public class CommonDiaryView : MonoBehaviour 
    {
        [SerializeField] private List<CommonDiaryPageView> pages;
        
        private Button nextPageButton;
        private Button previousPageButton;
        
        private List<Diary> currentDiaries;
        private int currentFirstDiaryIndex;
        private IStartMenuDiaryCommander commander;

        public void SetActive(bool value, Button nextBtn = null, Button prevBtn = null)
        {
            foreach (var page in pages)
            {
                page.SetActive(value);
            }
            
            if (value)
            {
                nextPageButton = nextBtn;
                previousPageButton = prevBtn;

                if (nextPageButton != null)
                {
                    nextPageButton.onClick.RemoveAllListeners();
                    nextPageButton.onClick.AddListener(MoveToNextPage);
                }
                if (previousPageButton != null)
                {
                    previousPageButton.onClick.RemoveAllListeners();
                    previousPageButton.onClick.AddListener(MoveToPreviousPage);
                }

                currentDiaries = commander.GetRecentDiaries();
                currentFirstDiaryIndex = 0;
                SetPages();
            }
        }

        public void MoveToNextPage()
        {
            if (!IsNextPageAvailable()) return;
            currentFirstDiaryIndex += pages.Count;
            SetPages();
        }

        public void MoveToPreviousPage()
        {
            if (!IsPreviousPageAvailable()) return;
            currentFirstDiaryIndex -= pages.Count;
            SetPages();
        }

        private bool IsNextPageAvailable()
        {
            return currentDiaries != null && currentFirstDiaryIndex + pages.Count < currentDiaries.Count;
        }

        private bool IsPreviousPageAvailable()
        {
            return currentFirstDiaryIndex - pages.Count >= 0;
        }

        private void SetPages()
        {
            for (int i = 0; i < pages.Count; i++)
            {
                int targetIndex = currentFirstDiaryIndex + i;
                if (targetIndex < currentDiaries.Count) pages[i].SetDiary(currentDiaries[targetIndex]);
                else pages[i].SetDiary(null);
            }
            
            UpdatePaginationButtons();
        }

        private void UpdatePaginationButtons()
        {
            if (nextPageButton != null) nextPageButton.gameObject.SetActive(IsNextPageAvailable());
            if (previousPageButton != null) previousPageButton.gameObject.SetActive(IsPreviousPageAvailable());
        }

        public void SetCommander(IStartMenuDiaryCommander commander)
        {
            this.commander = commander;
            foreach (var page in pages) page.SetCommander(commander);
        }
    }
}