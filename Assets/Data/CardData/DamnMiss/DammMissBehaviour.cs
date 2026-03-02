using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class DammMiss : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectEntity tooSlowEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DammMiss() {}
        private DammMiss(ICardBehaviourOwner owner, BattleStatusEffectEntity tooSlowEntity)
        : base(owner)
        {
            this.tooSlowEntity = tooSlowEntity;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new DammMiss(owner, tooSlowEntity);
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
            ExecuteCommonAction(context, target, 0.2);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, 0.4);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, double probability)
        {
            if (context.Random.NextDouble() > probability) { return; }

            var tooSlow = new BattleStatusEffect(tooSlowEntity, 1, 2);
            var action = new ApplyEntityStatusEffectBattleAction(target.Player, tooSlow);
            context.ActionScheduler.Enqueue(action);
        }
    }
}