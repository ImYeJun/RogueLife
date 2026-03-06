using System;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.IncidentSelectView
{
    public class IncidentSelectView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
    {
        [SerializeField] private GameObject uiRoot;

        public override void OnInitialized()
        {
            uiRoot.SetActive(false);

            eventBus.Subscribe<IncidentSelectRequested>(OnIncidentSelectRequested);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<IncidentSelectRequested>(OnIncidentSelectRequested);
        }

        public void OnIncidentSelectRequested(IncidentSelectRequested payload)
        {
            foreach (var choice in payload.Choices)
            {
                Debug.Log($"{choice.Description}");
            }
        }
    }
}
