using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class OmaeWaMoSindeIru : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectEntity omaeWaMoSindeIruEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OmaeWaMoSindeIru() {}
        private OmaeWaMoSindeIru(ICardBehaviourOwner owner, BattleStatusEffectEntity omaeWaMoSindeIruEntity, CardTargetType targetType, CardTargetType reflectionTargetType)
        : base(owner, targetType, reflectionTargetType)
        {
            this.omaeWaMoSindeIruEntity = omaeWaMoSindeIruEntity;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new OmaeWaMoSindeIru(owner, omaeWaMoSindeIruEntity, targetType, reflectionTargetType);
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
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, int stackCount)
        {
            var player = target.Player;
            var omaeWaMoSindeIru = new BattleStatusEffect(omaeWaMoSindeIruEntity, stackCount, 1);
            var applyBuffAction = new ApplyEntityStatusEffectBattleAction(player, omaeWaMoSindeIru);
            context.ActionScheduler.Enqueue(applyBuffAction);
        }
    }
}