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
                view.Initialize(viewEventBus);
            }
            foreach (var interactabelView in interacatbleViews)
            {
                interactabelView.Initialize(viewEventBus, viewCommander);
            }

            viewCommander.BroadcastCurrentState();
            viewCommander.EnterStartNodeIfNeeded();

            currentRun.ViewEventBus.Subscribe<RunEnded>(OnRunEnded);
        }
        private void OnDestroy()
        {
            currentRun?.ViewEventBus?.Unsubscribe<RunEnded>(OnRunEnded);
        }

        public void OnRunEnded(RunEnded payload)
        {
            GameSceneManager.Instance.LoadScene(SceneName.MAIN_MENU);
        }
    }
}