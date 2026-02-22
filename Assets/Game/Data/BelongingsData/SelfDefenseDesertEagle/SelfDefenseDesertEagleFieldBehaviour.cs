using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class FieldSelfDefenseDesertEagle : FieldBelongingsBehaviour
    {
        public override FieldBelongingsBehaviour Clone()
        {
            return new FieldSelfDefenseDesertEagle();
        }

        public override void OnEquipped(FieldContext context)
        {
        }

        public override void OnUnqeuipped(FieldContext context)
        {
        }
    }
}