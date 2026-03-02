using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class LowPointDefense : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectEntity defensiveStanceEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public LowPointDefense() {}
        private LowPointDefense(ICardBehaviourOwner owner, BattleStatusEffectEntity defensiveStanceEntity)
        : base(owner)
        {
            this.defensiveStanceEntity = defensiveStanceEntity;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new LowPointDefense(owner, defensiveStanceEntity);
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
            var player = target.Player;
            var defense = new BattleStatusEffect(defensiveStanceEntity, stackCount, 2);
            var applyBuffAction = new ApplyEntityStatusEffectBattleAction(player, defense);
            context.ActionScheduler.Enqueue(applyBuffAction);
        }
    }
}