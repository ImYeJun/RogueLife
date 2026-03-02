using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.ScheduleSelecting;
using TMPro;
using UnityEngine.EventSystems;

namespace View.ScheduleSelecting
{
    public class ScheduleSelectButton : 
        InteractableViewBehaviour<IScheduleSelectingEvent, ISelectingScheduleViewCommander>,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField] private GameObject onHoverImage;

        private ScheduleData data;
        private Image scheduleIcon;
        private TextMeshProUGUI text;

        public override void OnInitialized()
        {
            scheduleIcon = GetComponentInChildren<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();

            onHoverImage.SetActive(false);
        }

        public void SetData(ScheduleData data)
        {
            this.data = data;

            scheduleIcon.sprite = data.ChoiceSprite;
            text.text = data.ScheduleName;
        }

        public void OnPressed()
        {
            commander.SettleCurrentScheduleData(data);
        }

        public override void OnDestroy()
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onHoverImage.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onHoverImage.SetActive(false);
        }
    }
}
