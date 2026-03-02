using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class MysteriousAura : CardBattleBehaviour<BattleEntityCardTarget, BattleEntityCardTarget>
    {
        [SerializeField] BattleStatusEffectEntity strengthenMuscleEntity;
        [SerializeField] BattleStatusEffectEntity thatsWeakSpotEntity;
        [SerializeField] BattleStatusEffectEntity thatsFoulEntity;
        [SerializeField] BattleStatusEffectEntity ohMyEntity;


        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MysteriousAura() {}
        private MysteriousAura(ICardBehaviourOwner owner, BattleStatusEffectEntity strengthenMuscleEntity, BattleStatusEffectEntity thatsWeakSpotEntity, BattleStatusEffectEntity thatsFoulEntity, BattleStatusEffectEntity ohMyEntity)
        : base(owner)
        {
            this.strengthenMuscleEntity = strengthenMuscleEntity;
            this.thatsWeakSpotEntity = thatsWeakSpotEntity;
            this.thatsFoulEntity = thatsFoulEntity;
            this.ohMyEntity = ohMyEntity;
        }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new MysteriousAura(owner, strengthenMuscleEntity, thatsWeakSpotEntity, thatsFoulEntity, ohMyEntity);
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
                    var strengthenMuscle = new BattleStatusEffect(strengthenMuscleEntity, 4, 2);
                    determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, strengthenMuscle);
                    break;
                case 1:
                    var thatsWeakSpot = new BattleStatusEffect(thatsWeakSpotEntity, 4, 2);
                    determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, thatsWeakSpot);
                    break;
                case 2:
                    determinedAction = new HealEntityBattleAction(targetEntity, 10);
                    break;
                case 3:
                    determinedAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), 10, targetEntity);
                    break;
                case 4:
                    var thatsFoul = new BattleStatusEffect(thatsFoulEntity, 1, 2);
                    determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, thatsFoul);
                    break;
                case 5:
                    var ohMy = new BattleStatusEffect(ohMyEntity, 1, 2);
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
                        var strengthenMuscle = new BattleStatusEffect(strengthenMuscleEntity, 4, 2);
                        determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, strengthenMuscle);
                        break;
                    case 1:
                        determinedAction = new HealEntityBattleAction(targetEntity, 10);
                        break;
                    case 2:
                        var thatsFoul = new BattleStatusEffect(thatsFoulEntity, 1, 2);
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
                        var thatsWeakSpot = new BattleStatusEffect(thatsWeakSpotEntity, 4, 2);
                        determinedAction = new ApplyEntityStatusEffectBattleAction(targetEntity, thatsWeakSpot);
                        break;
                    case 1:
                        determinedAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), 10, targetEntity);
                        break;
                    case 2:
                        var ohMy = new BattleStatusEffect(ohMyEntity, 1, 2);
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