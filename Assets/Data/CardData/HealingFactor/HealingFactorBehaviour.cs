using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class HealingFactor : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectEntity superHealEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HealingFactor() {}
        private HealingFactor(ICardBehaviourOwner owner, BattleStatusEffectEntity superHealData) 
        : base(owner)
        {
            this.superHealEntity = superHealData;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new HealingFactor(owner, superHealEntity);
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
            ExecuteCommonAction(context, target, 2);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, 3);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, int duration)
        {
            var superHeal = new BattleStatusEffect(superHealEntity, 2, duration);
            var action = new ApplyEntityStatusEffectBattleAction(target.Player, superHeal);
            context.ActionScheduler.Enqueue(action);
        }
    }
}