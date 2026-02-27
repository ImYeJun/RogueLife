using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class DoubleOrNothing : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {   
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DoubleOrNothing() {}
        private DoubleOrNothing(ICardBehaviourOwner owner)
        : base(owner) { }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new DoubleOrNothing(owner);
        }

        public override bool OnIsAbleToUse(BattleContext context, NoneCardTarget target)
        {
            return true;
        }
        public override bool OnIsAbleToUseReflect(BattleContext context, NoneCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, 2);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, 3);
        }
        private void ExecuteCommonAction(BattleContext context, int drawCount)
        {
            var card = context.HandDeck.GetRandomCard(context.Random, owner);
            if (card is not null)
            {
                var moveCardAction = new MoveCardToDeckBattleAction(card, BattleDeckType.DRAW);
                context.ActionScheduler.Enqueue(moveCardAction);
            }

            if (context.Random.NextDouble() > 0.4) { return; }

            for (int i = 0; i < drawCount; i++)
            {
                var drawAction = new RequestDrawCardBattleAction(Guid.NewGuid());

                context.ActionScheduler.Enqueue(drawAction);
            }
        }
    }
}