using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class DamnMiss : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectData tooSlowData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DamnMiss() {}
        private DamnMiss(ICardBehaviourOwner owner, BattleStatusEffectData tooSlowData)
        : base(owner)
        {
            this.tooSlowData = tooSlowData;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new DamnMiss(owner, tooSlowData);
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

            var tooSlow = new BattleStatusEffect(tooSlowData, 1, 2);
            var action = new ApplyEntityStatusEffectBattleAction(target.Player, tooSlow);
            context.ActionScheduler.Enqueue(action);
        }
    }
}