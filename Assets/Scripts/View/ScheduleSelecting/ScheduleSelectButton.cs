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
        private ScheduleData data;
        private Image scheduleIcon;
        private TextMeshProUGUI text;

        public override void OnInitialized()
        {
            scheduleIcon = GetComponentInChildren<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetData(ScheduleData data)
        {
            this.data = data;

            scheduleIcon.sprite = data.ChoiceIdleSprite;
            text.text = data.ScheduleName;
        }

        public void OnPressed()
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector2 selectPos = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);

            commander.SettleCurrentScheduleData(data, selectPos);
        }

        public override void OnDestroy()
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            scheduleIcon.sprite = data.ChoiceHoveringSprite;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            scheduleIcon.sprite = data.ChoiceIdleSprite;
        }
    }
}
