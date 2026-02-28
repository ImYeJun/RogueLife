using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class DeckButton : ViewBehaviour<IScheduleViewEvent>
    {
        public override void OnInitialized()
        {
        }

        public override void OnDestroy()
        {
        }

        public void OnPressed()
        {
            UnityEngine.Debug.Log("덱 버튼 누름!");
        }
    }
}
