using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class BelongingsBagButton : ViewBehaviour<IScheduleViewEvent>
    {
        public override void OnInitialized()
        {
            // TODO: 이벤트 구독 (예: eventBus.Subscribe<T>(Method);)
        }

        public override void OnDestroy()
        {
            // TODO: 이벤트 구독 해제 (예: eventBus.Unsubscribe<T>(Method);)
        }

        public void OnPressed()
        {
            UnityEngine.Debug.Log("소지품 버튼 누름!");
        }
    }
}
