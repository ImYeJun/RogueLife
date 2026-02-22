using System;
using System.ComponentModel;
using UnityEngine;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class FieldBasicImplant : FieldBelongingsBehaviour
    {
        [SerializeField] Field.Deck.Observers.DecreaseCardActionCost decreaseCardActionCost;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FieldBasicImplant() {}
        public FieldBasicImplant(Field.Deck.Observers.DecreaseCardActionCost decreaseCardActionCost) {
            this.decreaseCardActionCost = decreaseCardActionCost;
        }

        public override FieldBelongingsBehaviour Clone()
        {
            return new FieldBasicImplant(decreaseCardActionCost);
        }

        public override void OnEquipped(FieldContext context)
        {
            context.Deck.RegisterDeckobserver(decreaseCardActionCost);
        }

        public override void OnUnqeuipped(FieldContext context)
        {
            context.Deck.UnrgisterDeckobserver(decreaseCardActionCost);
        }
    }
}