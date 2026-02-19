using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class ProteinSupplement : CardBattleBehaviour<NoneCardTarget, PlayerCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ProteinSupplement() {}
        private ProteinSupplement(ICardBehaviourOwner owner)
        : base(owner) {}
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new ProteinSupplement(owner);
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
            ExecuteCommonAction(context);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context);

            var player = target.Player;
            var action = new HealEntityBattleAction(player, 10);

            context.ActionScheduler.Enqueue(action);
        }
        private static void ExecuteCommonAction(BattleContext context)
        {
            var action = new RequestDrawCardBattleAction(CardRarity.ANY, CardAttribute.PHYSICAL, CardType.ANY, Guid.NewGuid());
            context.ActionScheduler.Enqueue(action);
        }
    }
}