using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.ScheduleView.BelongingsBag
{
    public class MainBelongingsSlotView : BelongingsSlotView
    {
        protected override void OnFocusedClicked()
        {
            commander.MoveBelonings(belongings, BelongingsBagType.MAIN_BELONGINGS_BAG, BelongingsBagType.SIDE_BELONGINGS_BAG);
        }
    }
}