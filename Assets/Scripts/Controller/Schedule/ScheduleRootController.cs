using System.Collections.Generic;
using System.Diagnostics;
using Controller.StartMenu;
using UnityEngine;
using View.Core;
using ViewEvent.ScheduleSelecting;
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

            viewEventBus.Subscribe((ScheduleStateSynced payload) =>
            {
                foreach (var deck in payload.Deck.MainDeck)
                {
                    // UnityEngine.Debug.Log($"{deck.Key.Id} : {deck.Value.Count}");
                }
            });

            viewCommander.BroadcastCurrentState();
            viewCommander.EnterStartNodeIfNeeded();
        }
    }
}