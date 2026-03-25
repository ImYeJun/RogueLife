using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.Core;
using ViewEvent.GameRunView;
using ViewEvent.ScheduleSelecting;

namespace Controller.SelectingSchedule
{
    public class SelectingScheduleRootController : InGameRootController
    {
        private ISelectingScheduleViewCommander viewCommander;
        private ScheduleSelectingViewEventBus viewEventBus;

        [SerializeField] private List<ViewBehaviour<IScheduleSelectingEvent>> views;
        [SerializeField] private List<InteractableViewBehaviour<IScheduleSelectingEvent, ISelectingScheduleViewCommander>> interacatbleViews;

        protected override void OnInitialize()
        { 
            viewCommander = currentRun.SelectingScheduleViewCommander;
            viewEventBus = currentRun.SelectingScheudleViewEventBus;

            foreach (var view in views)
            {
                view.Initialize(random, viewEventBus, PresentationManager.Instance);
            }
            foreach (var interactabelView in interacatbleViews)
            {
                interactabelView.Initialize(random, viewEventBus, PresentationManager.Instance ,viewCommander);
            }

            currentRun.ViewEventBus.Subscribe<RunEnded>(OnRunEnded);
            viewEventBus.Subscribe<ScheduleSettled>(OnScheduleSettled);

            currentRun.StartSchedule();
        }
        public void OnDestroy()
        {
            currentRun?.ViewEventBus?.Unsubscribe<RunEnded>(OnRunEnded);
            viewEventBus.Unsubscribe<ScheduleSettled>(OnScheduleSettled);
        }

        public void OnScheduleSettled(ScheduleSettled payload)
        {
            var presentationManager = PresentationManager.Instance;

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.ScheduleSettled_SceneTransition, LoadSceneRoutine());
        }

        public IEnumerator LoadSceneRoutine()
        {
            GameSceneManager.Instance.LoadScene(SceneName.SCHEDULE);

            yield return null;
        }
        
        public void OnRunEnded(RunEnded payload)
        {
            SceneName destination = payload.DiaryWritable ? SceneName.WRITE_DIARY : SceneName.MAIN_MENU;
            PresentationManager.Instance.Enqueue(payload.SequenceId, PresentationPriority.RunEnded_SceneTransition, SceneTransitionPresentation(destination));
        }
        public IEnumerator SceneTransitionPresentation(SceneName name)
        {
            yield return null;
            GameSceneManager.Instance.LoadScene(name);
        }
    }
}