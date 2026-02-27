using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class PainlessHit : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectData toughenData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PainlessHit() {}
        private PainlessHit(ICardBehaviourOwner owner, BattleStatusEffectData toughenData)
        : base(owner)
        {
            this.toughenData = toughenData;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new PainlessHit(owner, toughenData);
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
            var toughen = new BattleStatusEffect(toughenData, stackCount, 2);
            var action = new ApplyEntityStatusEffectBattleAction(target.Player, toughen);
            context.ActionScheduler.Enqueue(action);
        }
    }
}