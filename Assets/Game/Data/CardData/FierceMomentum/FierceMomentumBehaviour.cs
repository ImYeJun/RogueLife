using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class FierceMomentum : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        [SerializeField] private BattleStatusEffectData strengthenMuscleData;
        [SerializeField] private BattleStatusEffectData iWillKillYouData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FierceMomentum() {}
        private FierceMomentum(ICardBehaviourOwner owner, BattleStatusEffectData strengthenMuscleData, BattleStatusEffectData iWillKillYouData) 
        : base(owner)
        {
            this.strengthenMuscleData = strengthenMuscleData;
            this.iWillKillYouData = iWillKillYouData;
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new FierceMomentum(owner, strengthenMuscleData, iWillKillYouData);
        }

        public override bool OnIsAbleToUse(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }
        public override bool OnIsAbleToUseReflect(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, strengthenMuscleData);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, iWillKillYouData);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, BattleStatusEffectData statusEffectData)
        {
            var statusEffect = new BattleStatusEffect(statusEffectData, 3, 2);
            var applyStatusEffectAction = new ApplyEntityStatusEffectBattleAction(target.Player, statusEffect);
            context.ActionScheduler.Enqueue(applyStatusEffectAction);
        }
    }
}