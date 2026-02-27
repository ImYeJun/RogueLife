using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.ScheduleSelecting;
using TMPro;

namespace View.ScheduleSelecting
{
    public class ScheduleSelectButton : InteractableViewBehaviour<IScheduleSelectingEvent, ISelectingScheduleViewCommander>
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

            scheduleIcon.sprite = data.ChoiceSprite;
            text.text = data.Id;
        }

        public void OnPressed()
        {
            commander.SettleCurrentScheduleData(data);
        }

        public override void OnDestroy()
        {
        }
    }
}
