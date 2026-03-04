using UnityEngine;
using View.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.Map
{
    public class MapViewCloseButton : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private GameObject mapView;

        public override void OnInitialized()
        {
        }

        public override void OnDestroy()
        {
        }

        public void OnPressed()
        {
            mapView.SetActive(false);
        }
    }
}
