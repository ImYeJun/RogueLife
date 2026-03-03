using System;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.BelongingsBag
{
    public class BelongingsBagView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
    {
        private IReadOnlyBelongingsBag belongingsBag;

        public override void OnInitialized()
        {
            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
        }
        public void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            belongingsBag = payload.BelongingsBag;
            foreach (var belonging in belongingsBag.SideBelongingsBag)
            {
                UnityEngine.Debug.Log(belonging.Key.BelongingsName);
            }
        }

        public void OnViewOpened()
        {
            gameObject.SetActive(true);
        }
    }
}
