using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class MentalityView : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private Image mentalitySlider;
        [SerializeField] private TextMeshProUGUI mentalityText;

        public override void OnInitialized()
        {
            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
        }

        public override void OnDestroy()
        {
            eventBus.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
        }

        public void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            mentalitySlider.fillAmount = payload.Health.NomarlizedMentality;
            mentalityText.text = $"{payload.Health.CurrentMentality}/{payload.Health.MaxMentality}";
        }
    }
}
