using System.Collections.Generic;
using UnityEngine;
using View.Core;
using ViewEvent.ScheduleSelecting;

namespace Controller.SelectingSchedule
{
    public class RootController : SceneRootController
    {
        private ISelectingScheduleViewCommander viewCommander;
        private ScheduleSelectingViewEventBus viewEventBus;

        [SerializeField] private List<ViewBehaviour<IScheduleSelectingEvent>> views;
        [SerializeField] private List<InteractableViewBehaviour<IScheduleSelectingEvent, ISelectingScheduleViewCommander>> interacatbleViews;

        protected override void OnInitialize()
        { 
            viewCommander = currentRun.SelectingScheudleViewCommander;
            viewEventBus = currentRun.SelectingScheudleViewEventBus;

            foreach (var view in views)
            {
                view.Initialize(viewEventBus);
            }
            foreach (var interactabelView in interacatbleViews)
            {
                interactabelView.Initialize(viewEventBus, viewCommander);
            }

            currentRun.StartSchedule();
        }
    }
}