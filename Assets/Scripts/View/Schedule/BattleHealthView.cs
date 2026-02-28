using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class BattleHealthView : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private Image battleHealthSlider;
        [SerializeField] private TextMeshProUGUI battleHealthText;

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
            battleHealthSlider.fillAmount = payload.Health.NormalizedBattleHealth;
            battleHealthText.text = $"{payload.Health.CurrentBattleHealth}/{payload.Health.MaxBattleHealth}";
        }
    }
}
