using UnityEngine;
using UnityEngine.EventSystems;
using View.Core;
using ViewEvent.ScheduleSelecting;

namespace View.ScheduleSelecting
{
    public class ScheduleEndButton : 
        InteractableViewBehaviour<IScheduleSelectingEvent, ISelectingScheduleViewCommander>,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField] private GameObject onHoverImage;

        public override void OnDestroy()
        {
            
        }

        public override void OnInitialized()
        {
            onHoverImage.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onHoverImage.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onHoverImage.SetActive(false);
        }

        public void OnPressed()
        {
            commander.UnsettleSchedule();
        }
    }
}