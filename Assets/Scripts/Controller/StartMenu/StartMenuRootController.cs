using System;
using System.Collections.Generic;
using UnityEngine;
using View.Core;
using View.StartMenu;
using ViewEvent.ScheduleSelecting;
using ViewEvent.StartMenu;

namespace Controller.StartMenu
{
    public class StartMenuRootConroller : SceneRootController
    {
        [SerializeField] private MainMenuManager mainMenuManager;
        [SerializeField] private Transform startDeckSelectView;
        [SerializeField] private GameObject startDeckSelectButtonPrefab;

        private IStartMenuViewCommander viewCommander;
        private StartMenuViewEventBus viewEventBus;

        [SerializeField] private List<ViewBehaviour<IStartMenuViewEvent>> views;
        [SerializeField] private List<InteractableViewBehaviour<IStartMenuViewEvent, IStartMenuViewCommander>> interacatbleViews;
        
        protected override void OnInitialize()
        {
            viewCommander = mainMenuManager;
            viewEventBus = mainMenuManager.ViewEventBus;

            foreach (var view in views)
            {
                view.Initialize(viewEventBus);
            }
            foreach (var interactabelView in interacatbleViews)
            {
                interactabelView.Initialize(viewEventBus, viewCommander);
            }

            viewEventBus.Subscribe<ReadyToStartGame>(OnReadyToStartGame);
            viewEventBus.Subscribe<StartDeckLoaded>(OnStartDeckLoaded);
        }

        private void OnDestroy()
        {
            viewEventBus?.Unsubscribe<ReadyToStartGame>(OnReadyToStartGame);
            viewEventBus?.Unsubscribe<StartDeckLoaded>(OnStartDeckLoaded);
        }

        public void OnReadyToStartGame(ReadyToStartGame payload)
        {
            GameSceneManager.Instance.LoadScene(SceneName.SCHEDULE_SELECTING);
        }

        private void OnStartDeckLoaded(StartDeckLoaded payload)
        {
            foreach (var startDeck in payload.StartDecks)
            {
                var button = Instantiate(startDeckSelectButtonPrefab);
                
                var startDeckSelectButton = button.GetComponent<StartDeckSelectButton>();

                startDeckSelectButton.SetStartDeck(startDeck);
                button.transform.SetParent(startDeckSelectView);

                startDeckSelectButton.Initialize(viewEventBus, viewCommander);
            }
        }
    }
}