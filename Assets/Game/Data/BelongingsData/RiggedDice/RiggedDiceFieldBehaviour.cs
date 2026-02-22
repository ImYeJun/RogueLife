using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class FieldRiggedDice : FieldBelongingsBehaviour
    {
        public override FieldBelongingsBehaviour Clone()
        {
            return new FieldRiggedDice();
        }

        public override void OnEquipped(FieldContext context)
        {
        }

        public override void OnUnqeuipped(FieldContext context)
        {
        }
    }
}