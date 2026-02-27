using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class FieldCarrotCarrot : FieldBelongingsBehaviour
    {
        public override FieldBelongingsBehaviour Clone()
        {
            return new FieldCarrotCarrot();
        }

        public override void OnEquipped(FieldContext context)
        {
        }

        public override void OnUnqeuipped(FieldContext context)
        {
        }
    }
}