using UnityEngine;
using View.Core;
using View.ScheduleView.BelongingsBag;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class BelongingsBagButton : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private BelongingsBagView belongingsBagView;

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
            belongingsBagView.OnViewOpened();
        }
    }
}
