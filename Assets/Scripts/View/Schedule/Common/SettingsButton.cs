using UI.Global;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class SettingsButton : ViewBehaviour<IScheduleViewEvent>
    {
        public override void OnInitialized()
        {
        }

        public override void OnDestroy()
        {
        }

        public void OnPressed()
        {
            GlobalUIManager.Instance.OpenSettingUI();
        }
    }
}
