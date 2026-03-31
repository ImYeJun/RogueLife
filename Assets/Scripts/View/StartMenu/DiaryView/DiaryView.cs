using System;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.StartMenu;

namespace View.StartMenu.DiaryViews
{
    public class DiaryView : InteractableViewBehaviour<IStartMenuViewEvent, IStartMenuViewCommander>
    {
        [SerializeField] private GameObject diaryView;
        [SerializeField] private CommonDiaryView commonDiaryView;
        [SerializeField] private SpecialDiaryView specialDiaryView;

        [Header("Shared Pagination Buttons")]
        [SerializeField] private Button sharedNextPageButton;
        [SerializeField] private Button sharedPreviousPageButton;

        private void Awake() {
            diaryView.SetActive(false);
            sharedNextPageButton.gameObject.SetActive(false);
            sharedPreviousPageButton.gameObject.SetActive(false);
        }

        public override void OnInitialized()
        {
            commonDiaryView.SetCommander(commander);
            specialDiaryView.SetCommander(commander);

            commonDiaryView.SetActive(false);
            specialDiaryView.SetActive(false);
            
            sharedNextPageButton.gameObject.SetActive(false);
            sharedPreviousPageButton.gameObject.SetActive(false);
        }

        public override void OnDestroy()
        {
        }

        public void OnOpened()
        {
            diaryView.SetActive(true);
            OpenCommon();
        }

        public void OnClosed()
        {
            diaryView.SetActive(false);
        }

        public void OpenCommon()
        {
            specialDiaryView.SetActive(false);
            commonDiaryView.SetActive(true, sharedNextPageButton, sharedPreviousPageButton);
        }

        public void OpenSpecial()
        {
            commonDiaryView.SetActive(false);
            specialDiaryView.SetActive(true, sharedNextPageButton, sharedPreviousPageButton);
        }
    }
}