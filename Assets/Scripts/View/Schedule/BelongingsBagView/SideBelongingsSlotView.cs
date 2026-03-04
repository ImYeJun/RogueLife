using UnityEngine;

namespace View.ScheduleView.BelongingsBag
{
    public class SideBelongingsSlotView : BelongingsSlotView
    {
        protected override void OnFocusedClicked()
        {
            commander.MoveBelonings(belongings, BelongingsBagType.SIDE_BELONGINGS_BAG, BelongingsBagType.MAIN_BELONGINGS_BAG);
        }
    }
}