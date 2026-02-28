using TMPro;
using View.Core;
using ViewEvent.ScheduleSelecting;

namespace View.ScheduleSelecting
{
    public class ScheduleCountIndicatorText : ViewBehaviour<IScheduleSelectingEvent>
    {
        private TextMeshProUGUI text;

        public override void OnDestroy()
        {
            eventBus.Unsubscribe<ReadyToSelectSchedule>(OnReadyToSelectSchedule);
        }

        public override void OnInitialized()
        {
            text = GetComponent<TextMeshProUGUI>();

            eventBus.Subscribe<ReadyToSelectSchedule>(OnReadyToSelectSchedule);
        }

        public void OnReadyToSelectSchedule(ReadyToSelectSchedule payload)
        {
            int scheduleCount = payload.CurrentStartCount;

            text.text = $"Starting {scheduleCount} Schedule";
        }
    }
}