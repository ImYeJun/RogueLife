using UnityEngine;
using UnityEngine.EventSystems;
using View.Core;
using ViewEvent.ScheduleSelecting;

namespace View.ScheduleSelecting
{
    public class ScheduleEndButton : InteractableViewBehaviour<IScheduleSelectingEvent, ISelectingScheduleViewCommander>
    {
        public override void OnDestroy()
        {
            
        }

        public override void OnInitialized()
        {
        }

        public void OnPressed()
        {
            commander.UnsettleSchedule();
        }
    }
}