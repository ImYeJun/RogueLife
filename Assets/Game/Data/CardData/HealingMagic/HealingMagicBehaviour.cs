using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class HealingMagic : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HealingMagic() {}
        private HealingMagic(ICardBehaviourOwner owner) 
        : base(owner)
        { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new HealingMagic(owner);
        }

        public override bool IsAbleToUse(BattleContext context, CardTarget target)
        {
            return true;
        }

        public override bool IsAbleToUseReflect(BattleContext context, CardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, 35);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, 50);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, int healAmount)
        {
            var healAction = new HealEntityBattleAction(target.Player, healAmount);
            context.ActionScheduler.Enqueue(healAction);
        }
    }
}