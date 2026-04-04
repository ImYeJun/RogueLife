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
        private HealingMagic(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType)
        { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new HealingMagic(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, 25);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, 40);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, int healAmount)
        {
            var healAction = new HealEntityBattleAction(target.Player, healAmount);
            context.ActionScheduler.Enqueue(healAction);
        }
    }
}