using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class PerfectDisposableBarrier : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectEntity thatsFoulEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PerfectDisposableBarrier() {}
        private PerfectDisposableBarrier(ICardBehaviourOwner owner, BattleStatusEffectEntity thatsFoulEntity) 
        : base(owner)
        {
            this.thatsFoulEntity = thatsFoulEntity;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new PerfectDisposableBarrier(owner, thatsFoulEntity);
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
            var thatsFoul = new BattleStatusEffect(thatsFoulEntity, 1, 2);
            var action = new ApplyEntityStatusEffectBattleAction(target.Player, thatsFoul);
            context.ActionScheduler.Enqueue(action);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            var thatsFoul = new BattleStatusEffect(thatsFoulEntity, 1);
            var action = new ApplyEntityStatusEffectBattleAction(target.Player, thatsFoul);
            context.ActionScheduler.Enqueue(action);
        }
    }
}