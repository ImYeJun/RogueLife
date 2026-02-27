using System;
using System.ComponentModel;
using UnityEngine;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class FieldSmartLookingGlasses : FieldBelongingsBehaviour
    {
        [SerializeField] Field.Deck.Observers.DecreaseCardActionCost decreaseCardActionCost;
        [SerializeField] Field.Deck.Observers.IncreaseCardActionCost increaseCardActionCost;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FieldSmartLookingGlasses() {}

        private FieldSmartLookingGlasses(Field.Deck.Observers.DecreaseCardActionCost decreaseCardActionCost, Field.Deck.Observers.IncreaseCardActionCost increaseCardActionCost)
        {
            this.decreaseCardActionCost = decreaseCardActionCost;
            this.increaseCardActionCost = increaseCardActionCost;
        }

        public override FieldBelongingsBehaviour Clone()
        {
            return new FieldSmartLookingGlasses(decreaseCardActionCost, increaseCardActionCost);
        }

        public override void OnEquipped(FieldContext context)
        {
            context.Deck.RegisterDeckobserver(decreaseCardActionCost);
            context.Deck.RegisterDeckobserver(increaseCardActionCost);
        }

        public override void OnUnqeuipped(FieldContext context)
        {
            context.Deck.UnrgisterDeckobserver(decreaseCardActionCost);
            context.Deck.UnrgisterDeckobserver(increaseCardActionCost);
        }
    }
}