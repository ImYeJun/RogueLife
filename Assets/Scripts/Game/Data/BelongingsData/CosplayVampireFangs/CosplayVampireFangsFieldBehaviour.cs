using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class FieldCosplayVampireFangs : FieldBelongingsBehaviour
    {
        public override FieldBelongingsBehaviour Clone()
        {
            return new FieldCosplayVampireFangs();
        }

        public override void OnEquipped(FieldContext context)
        {
        }

        public override void OnUnqeuipped(FieldContext context)
        {
        }
    }
}