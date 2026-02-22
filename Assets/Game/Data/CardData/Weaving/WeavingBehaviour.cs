using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class Weaving : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] BattleStatusEffectData tooSlowData;
        [SerializeField] BattleStatusEffectData strengthenMuscleData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Weaving() {}
        private Weaving(ICardBehaviourOwner owner, BattleStatusEffectData tooSlowData, BattleStatusEffectData strengthenMuscleData)
        : base(owner)
        {
            this.tooSlowData = tooSlowData;
            this.strengthenMuscleData = strengthenMuscleData;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new Weaving(owner, tooSlowData, strengthenMuscleData);
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
            ExecuteCommonAction(context, target);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target);

            var player = target.Player;
            var strengthenMuscle = new BattleStatusEffect(strengthenMuscleData, 2, 2);
            var applyBuffAction = new ApplyEntityStatusEffectBattleAction(player, strengthenMuscle);
            context.ActionScheduler.Enqueue(applyBuffAction);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target)
        {
            var player = target.Player;
            var tooSlow = new BattleStatusEffect(tooSlowData, 1, 2);
            var applyBuffAction = new ApplyEntityStatusEffectBattleAction(player, tooSlow);
            context.ActionScheduler.Enqueue(applyBuffAction);
        }
    }
}