using System;
using System.Collections;
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
                view.Initialize(random, viewEventBus, PresentationManager.Instance);
            }
            foreach (var interactabelView in interacatbleViews)
            {
                interactabelView.Initialize(random, viewEventBus, PresentationManager.Instance ,viewCommander);
            }

            viewCommander.BroadcastCurrentState();
            viewCommander.EnterStartNodeIfNeeded();

            currentRun.ViewEventBus.Subscribe<RunEnded>(OnRunEnded);
            currentRun.ViewEventBus.Subscribe<ScheduleCleared>(OnScheduleCleared);
            viewEventBus.Subscribe<BattleEngaged>(OnBattleEngaged);
        }
        private void OnDestroy()
        {
            currentRun?.ViewEventBus?.Unsubscribe<RunEnded>(OnRunEnded);
            currentRun?.ViewEventBus?.Unsubscribe<ScheduleCleared>(OnScheduleCleared);
            viewEventBus?.Unsubscribe<BattleEngaged>(OnBattleEngaged);
        }

        public void OnRunEnded(RunEnded payload)
        {
            GameSceneManager.Instance.LoadScene(SceneName.MAIN_MENU);
        }
        public void OnScheduleCleared(ScheduleCleared payloaed)
        {
            GameSceneManager.Instance.LoadScene(SceneName.SCHEDULE_SELECTING);
        }
        public void OnBattleEngaged(BattleEngaged payload)
        {
            PresentationManager.Instance.Enqueue(payload.SequenceId, PresentationPriority.BattleEngaged_SceneTransition, BattleSceneTransitionPresentation());
        }

        public IEnumerator BattleSceneTransitionPresentation()
        {
            yield return null;
            GameSceneManager.Instance.LoadScene(SceneName.BATTLE);
        }
    }
}