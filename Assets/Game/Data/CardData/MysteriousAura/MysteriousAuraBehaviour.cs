using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class MysteriousAura : CardBattleBehaviour<BattleEntityCardTarget, BattleEntityCardTarget>
    {
        [SerializeField] BattleStatusEffectData strengthenMuscleData;
        [SerializeField] BattleStatusEffectData thatsWeakSpotData;
        [SerializeField] BattleStatusEffectData thatsFoulData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MysteriousAura() {}
        private MysteriousAura(ICardBehaviourOwner owner, BattleStatusEffectData strengthenMuscleData, BattleStatusEffectData thatsWeakSpotData, BattleStatusEffectData thatsFoulData)
        : base(owner)
        {
            this.strengthenMuscleData = strengthenMuscleData;
            this.thatsWeakSpotData = thatsWeakSpotData;
            this.thatsFoulData = thatsFoulData;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new MysteriousAura(owner, strengthenMuscleData, thatsWeakSpotData, thatsFoulData);
        }

        public override bool OnIsAbleToUse(BattleContext context, BattleEntityCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, BattleEntityCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, BattleEntityCardTarget target)
        {
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, BattleEntityCardTarget target)
        {
        }
    }
}