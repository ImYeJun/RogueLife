using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class AbsGuard : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectData defensiveStanceData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AbsGuard() {}
        private AbsGuard(ICardBehaviourOwner owner, BattleStatusEffectData defensiveStanceData)
        : base(owner)
        {
            this.defensiveStanceData = defensiveStanceData;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new AbsGuard(owner, defensiveStanceData);
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
            var defense = new BattleStatusEffect(defensiveStanceData, stackCount, 2);
            var applyBuffAction = new ApplyEntityStatusEffectBattleAction(player, defense);
            context.ActionScheduler.Enqueue(applyBuffAction);
        }
    }
}