using System;
using System.Collections;
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
        [SerializeField] private StartMenuManager mainMenuManager;

        private IStartMenuViewCommander viewCommander;
        private StartMenuViewEventBus viewEventBus;
        private System.Random random;

        [SerializeField] private List<ViewBehaviour<IStartMenuViewEvent>> views;
        [SerializeField] private List<InteractableViewBehaviour<IStartMenuViewEvent, IStartMenuViewCommander>> interacatbleViews;
        
        protected override void OnInitialize()
        {
            mainMenuManager.Initialize();

            viewCommander = mainMenuManager;
            viewEventBus = mainMenuManager.ViewEventBus;

            foreach (var view in views)
            {
                view.Initialize(viewEventBus, PresentationManager.Instance);
            }
            foreach (var interactabelView in interacatbleViews)
            {
                interactabelView.Initialize(viewEventBus, PresentationManager.Instance, viewCommander);
            }
            viewEventBus.Subscribe<ReadyToStartGame>(OnReadyToStartGame);
        }

        private void OnDestroy()
        {
            viewEventBus?.Unsubscribe<ReadyToStartGame>(OnReadyToStartGame);
        }

        public void OnReadyToStartGame(ReadyToStartGame payload)
        {

            PresentationManager.Instance.Enqueue(payload.SequenceId, PresentationPriority.ReadyToStartGame_SceneTransition, SceneTransitionPresentation(), () =>
            {
                GameSceneManager.Instance.LoadScene(SceneName.SCHEDULE_SELECTING);
            });
        }

        public IEnumerator SceneTransitionPresentation()
        {
            yield return null;
        }
    }
}