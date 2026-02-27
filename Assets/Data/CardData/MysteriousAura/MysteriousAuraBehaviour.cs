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
        [SerializeField] BattleStatusEffectData ohMyData;


        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MysteriousAura() {}
        private MysteriousAura(ICardBehaviourOwner owner, BattleStatusEffectData strengthenMuscleData, BattleStatusEffectData thatsWeakSpotData, BattleStatusEffectData thatsFoulData, BattleStatusEffectData ohMyData)
        : base(owner)
        {
            this.strengthenMuscleData = strengthenMuscleData;
            this.thatsWeakSpotData = thatsWeakSpotData;
            this.thatsFoulData = thatsFoulData;
            this.ohMyData = ohMyData;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new MysteriousAura(owner, strengthenMuscleData, thatsWeakSpotData, thatsFoulData, ohMyData);
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
            var randomNumber = context.Random.Next(6);
            
            IBattleAction determinedAction;
            var targetEntity = target.Entity;
            switch (randomNumber)
            {
                case 0:
                    var strengthenMuscle = new BattleStatusEffect(strengthenMuscleData, 4, 2);
                    determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, strengthenMuscle);
                    break;
                case 1:
                    var thatsWeakSpot = new BattleStatusEffect(thatsWeakSpotData, 4, 2);
                    determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, thatsWeakSpot);
                    break;
                case 2:
                    determinedAction = new HealEntityBattleAction(targetEntity, 10);
                    break;
                case 3:
                    determinedAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), 10, targetEntity);
                    break;
                case 4:
                    var thatsFoul = new BattleStatusEffect(thatsFoulData, 1, 2);
                    determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, thatsFoul);
                    break;
                case 5:
                    var ohMy = new BattleStatusEffect(ohMyData, 1, 2);
                    determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, ohMy);
                    break;
                default:
                    throw new InvalidOperationException("[MysteriousAura] What?? check the random number.");
            }

            context.ActionScheduler.Enqueue(determinedAction);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, BattleEntityCardTarget target)
        {
            var randomNumber = context.Random.Next(3);
            
            IBattleAction determinedAction;
            var targetEntity = target.Entity;

            if (targetEntity == caster.Caster)
            {
                switch (randomNumber)
                {
                    case 0:
                        var strengthenMuscle = new BattleStatusEffect(strengthenMuscleData, 4, 2);
                        determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, strengthenMuscle);
                        break;
                    case 1:
                        determinedAction = new HealEntityBattleAction(targetEntity, 10);
                        break;
                    case 2:
                        var thatsFoul = new BattleStatusEffect(thatsFoulData, 1, 2);
                        determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, thatsFoul);
                        break;
                    default:
                        throw new InvalidOperationException("[MysteriousAura] What?? check the random number.");
                }
            }
            else
            {
                switch (randomNumber)
                {
                    case 0:
                        var thatsWeakSpot = new BattleStatusEffect(thatsWeakSpotData, 4, 2);
                        determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, thatsWeakSpot);
                        break;
                    case 1:
                        determinedAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), 10, targetEntity);
                        break;
                    case 2:
                        var ohMy = new BattleStatusEffect(ohMyData, 1, 2);
                        determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, ohMy);
                        break;
                    default:
                        throw new InvalidOperationException("[MysteriousAura] What?? check the random number.");
                }
            }

            context.ActionScheduler.Enqueue(determinedAction);
        }
    }
}