using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.BelongingsBag
{
    public class BelongingsInspectorView : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private Image belongingsIcon;
        [SerializeField] private TextMeshProUGUI belongingsName;
        [SerializeField] private TextMeshProUGUI belongingsEffectDescription;
        private Belongings currentBelongings;

        public override void OnDestroy()
        {
        }
        public override void OnInitialized()
        {
            SetViewActive(false);
        }
        public void SetViewActive(bool value)
        {
            belongingsIcon.gameObject.SetActive(value);
            belongingsName.gameObject.SetActive(value);
            belongingsEffectDescription.gameObject.SetActive(value);
        }

        //* Referenced by BelongingsSlotView in UnityEvent 
        public void VisualizeSelectedSlot(Belongings belongings)
        {
            currentBelongings = belongings;
            
            belongingsIcon.sprite = belongings.Image;
            belongingsName.text = belongings.Name;
            belongingsEffectDescription.text = $"효과 : \n {belongings.Description}";

            SetViewActive(true);
        }
    }
}