using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class Ryujia108Stairs40Combo : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Ryujia108Stairs40Combo() {}
        private Ryujia108Stairs40Combo(ICardBehaviourOwner owner)
        : base(owner) {}
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new Ryujia108Stairs40Combo(owner);
        }

        public override bool IsAbleToUse(BattleContext context, CardTarget target)
        {
            return true;
        }

        public override bool IsAbleToUseReflect(BattleContext context, CardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            var cards = context.HandDeck.GetCardsByCondition(CardRarity.ANY, CardAttribute.PHYSICAL, CardType.ANY)
                                .Where(card => card != owner)
                                .ToList();
            ExecuteCommonAction(context, cards);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            var cards = context.HandDeck.GetCards()
                        .Where(card => card != owner)
                        .ToList();
            ExecuteCommonAction(context, cards);
        }
        private static void ExecuteCommonAction(BattleContext context, List<Card> cards)
        {
            foreach (var card in cards)
            {
                //TODO Delegate the UseCardBattleAction action to UI
                var cardUseAction = new UseCardBattleAction(card, null);
                // context.ActionScheduler.Enqueue(cardUseAction);
                UnityEngine.Debug.LogError("[Ryujia108Stairs40Combo] Delegate the reponsibility!!!");
            }
        }
    }
}