using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class BattleBelovedMyStone : BattleBelongingsBehaviour
    {
        public override BattleBelongingsBehaviour Clone()
        {
            return new BattleBelovedMyStone();
        }

        protected override void OnApplied()
        {
        }

        protected override void OnRemoved()
        {
        }
    }
}