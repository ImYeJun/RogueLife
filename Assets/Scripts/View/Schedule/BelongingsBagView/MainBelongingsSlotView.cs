using System;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Global;

namespace View.ScheduleView.BelongingsBag
{
    public class MainBelongingsSlotView : BelongingsSlotView
    {
        [SerializeField] private ButtonJuice buttonJuice;

        public bool IsFeedbackable { get => buttonJuice.IsFeedbackable; set => buttonJuice.IsFeedbackable = value; }

        protected override void OnFocusedClicked()
        {
            commander.MoveBelonings(belongings, BelongingsBagType.MAIN_BELONGINGS_BAG, BelongingsBagType.SIDE_BELONGINGS_BAG);
        }
    }
}