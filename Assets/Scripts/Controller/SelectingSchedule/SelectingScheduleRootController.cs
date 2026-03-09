using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.Core;
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

            viewEventBus.Subscribe<ScheduleSettled>(OnScheduleSettled);

            currentRun.StartSchedule();
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

        public void OnDestroy()
        {
            viewEventBus.Unsubscribe<ScheduleSettled>(OnScheduleSettled);
        }
    }
}