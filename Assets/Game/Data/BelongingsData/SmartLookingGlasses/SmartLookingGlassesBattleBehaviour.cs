using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class BattleSmartLookingGlasses : BattleBelongingsBehaviour
    {
        public override BattleBelongingsBehaviour Clone()
        {
            return new BattleSmartLookingGlasses();
        }

        protected override void OnApplied()
        {
        }

        protected override void OnRemoved()
        {
        }
    }
}