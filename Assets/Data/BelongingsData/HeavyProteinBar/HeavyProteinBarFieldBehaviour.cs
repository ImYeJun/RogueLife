using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class FieldHeavyProteinBar : FieldBelongingsBehaviour
    {
        public override FieldBelongingsBehaviour Clone()
        {
            return new FieldHeavyProteinBar();
        }

        public override void OnEquipped(FieldContext context)
        {
        }

        public override void OnUnqeuipped(FieldContext context)
        {
        }
    }
}