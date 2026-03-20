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

            currentRun.ViewEventBus.Subscribe<RunEnded>(OnRunEnded);
            currentRun.ViewEventBus.Subscribe<ScheduleCleared>(OnScheduleCleared);
            viewEventBus.Subscribe<BattleEngaged>(OnBattleEngaged);

            viewCommander.BroadcastCurrentState();
            viewCommander.ResumeSchedule();
        }
        private void OnDestroy()
        {
            currentRun?.ViewEventBus?.Unsubscribe<RunEnded>(OnRunEnded);
            currentRun?.ViewEventBus?.Unsubscribe<ScheduleCleared>(OnScheduleCleared);
            viewEventBus?.Unsubscribe<BattleEngaged>(OnBattleEngaged);
        }

        public void OnRunEnded(RunEnded payload)
        {
            SceneName destination = payload.DiaryWritable ? SceneName.WRITE_DIARY : SceneName.MAIN_MENU;
            PresentationManager.Instance.Enqueue(payload.SequenceId, PresentationPriority.GameEnded_SceneTransition, SceneTransitionPresentation(destination));
        }
        public void OnScheduleCleared(ScheduleCleared payload)
        {
            PresentationManager.Instance.Enqueue(payload.SequenceId, PresentationPriority.ScheduleCleared_SceneTransition, SceneTransitionPresentation(SceneName.SCHEDULE_SELECTING));
        }
        public void OnBattleEngaged(BattleEngaged payload)
        {
            PresentationManager.Instance.Enqueue(payload.SequenceId, PresentationPriority.BattleEngaged_SceneTransition, SceneTransitionPresentation(SceneName.BATTLE));
        }

        public IEnumerator SceneTransitionPresentation(SceneName name)
        {
            yield return null;
            GameSceneManager.Instance.LoadScene(name);
        }
    }
}