using TMPro;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class ScheduleCountView : ViewBehaviour<IScheduleViewEvent>
    {
        private TextMeshProUGUI text;

        public override void OnInitialized()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();

            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
        }

        public override void OnDestroy()
        {
            eventBus.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
        }

        public void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            text.text = $"{payload.CurrentScheduleCount} Schedule : {payload.CurrentScheduleData.Id}";
        }
    }
}
