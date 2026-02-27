using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class FieldBelovedMyStone : FieldBelongingsBehaviour
    {
        public override FieldBelongingsBehaviour Clone()
        {
            return new FieldBelovedMyStone();
        }

        public override void OnEquipped(FieldContext context)
        {
        }

        public override void OnUnqeuipped(FieldContext context)
        {
        }
    }
}