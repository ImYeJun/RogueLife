using System;
using System.Collections.Generic;
using UnityEngine;
using View.Core;
using ViewEvent.GameRunView;
using ViewEvent.ScheduleView;

namespace Controller.Schedule
{
    public class ScheduleRootController : InGameRootController
    {
        private ScheduleViewEventBus viewEventBus;
        private IScheduleViewCommander viewCommander;

        [SerializeField] private List<ViewBehaviour<IScheduleViewEvent>> views;
        [SerializeField] private List<InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>> interacatbleViews;

        protected override void OnInitialize()
        {
            viewCommander = currentRun.ScheduleViewCommander;
            viewEventBus = currentRun.ScheduleViewEventBus;

            foreach (var view in views)
            {
                view.Initialize(viewEventBus, PresentationManager.Instance);
            }
            foreach (var interactabelView in interacatbleViews)
            {
                interactabelView.Initialize(viewEventBus, PresentationManager.Instance ,viewCommander);
            }

            viewCommander.BroadcastCurrentState();
            viewCommander.EnterStartNodeIfNeeded();

            currentRun.ViewEventBus.Subscribe<RunEnded>(OnRunEnded);
            currentRun.ViewEventBus.Subscribe<ScheduleCleared>(OnScheduleCleared);
        }
        private void OnDestroy()
        {
            currentRun?.ViewEventBus?.Unsubscribe<RunEnded>(OnRunEnded);
            currentRun?.ViewEventBus?.Unsubscribe<ScheduleCleared>(OnScheduleCleared);
        }

        public void OnRunEnded(RunEnded payload)
        {
            GameSceneManager.Instance.LoadScene(SceneName.MAIN_MENU);
        }
        private void OnScheduleCleared(ScheduleCleared ended)
        {
            GameSceneManager.Instance.LoadScene(SceneName.SCHEDULE_SELECTING);
        }
    }
}