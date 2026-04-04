using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class PainlessHit : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectEntity toughenEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PainlessHit() {}
        private PainlessHit(ICardBehaviourOwner owner, BattleStatusEffectEntity toughenEntity, CardTargetType targetType, CardTargetType reflectionTargetType)
        : base(owner, targetType, reflectionTargetType)
        {
            this.toughenEntity = toughenEntity;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new PainlessHit(owner, toughenEntity, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, 3);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, 4);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, int stackCount)
        {
            var toughen = new BattleStatusEffect(toughenEntity, stackCount, 2);
            var action = new ApplyEntityStatusEffectBattleAction(target.Player, toughen);
            context.ActionScheduler.Enqueue(action);
        }
    }
}