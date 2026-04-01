using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class Background : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private Image background;

        public override void OnInitialized()
        {
            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
        }

        private void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            background.sprite = payload.Schedule.Data.UsualBackground;
        }
    }   
}
