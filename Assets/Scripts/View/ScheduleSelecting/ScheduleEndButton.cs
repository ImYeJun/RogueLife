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
            UnityEngine.Debug.Log("ToDo : 일기 쓰기 기능 구현하기");
        }
    }
}