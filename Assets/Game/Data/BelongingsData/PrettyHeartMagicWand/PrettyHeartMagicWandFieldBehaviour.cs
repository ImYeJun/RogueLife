using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class FieldPrettyHeartMagicWand : FieldBelongingsBehaviour
    {
        public override FieldBelongingsBehaviour Clone()
        {
            return new FieldPrettyHeartMagicWand();
        }

        public override void OnEquipped(FieldContext context)
        {
        }

        public override void OnUnqeuipped(FieldContext context)
        {
        }
    }
}