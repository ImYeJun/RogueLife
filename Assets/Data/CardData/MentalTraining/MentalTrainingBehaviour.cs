using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class MentalTraining : CardBattleBehaviour<PlayerCardTarget, NoneCardTarget>
    {
        private class Observer
        {
            private BattleContext context;
            private int remainObserveCount;

            public Observer(BattleContext context, int remainObserveCount)
            {
                this.context = context;
                this.remainObserveCount = remainObserveCount;
            }

            public void ModifyTryUseCard(TryUseCardBattleAction tryUseCard, BattleContext context)
            {
                tryUseCard.ReduceCost(1);

                if(--remainObserveCount <= 0)
                {
                    CleanItSelf();
                }
            }

            public void OnBattleEnd(BattleEndBattleEvent payload)
            {
                CleanItSelf();
            }

            private void CleanItSelf()
            {
                context.ActionObserverHub.UnsubscribeActionModifier<TryUseCardBattleAction>(ModifyTryUseCard);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

        [SerializeField] private BattleStatusEffectEntity lightBodyEntity;
        
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MentalTraining() {}
        private MentalTraining(ICardBehaviourOwner owner, BattleStatusEffectEntity lightBodyEntity, CardTargetType targetType, CardTargetType reflectionTargetType) : base(owner, targetType, reflectionTargetType) 
        { 
            this.lightBodyEntity = lightBodyEntity;
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new MentalTraining(owner, this.lightBodyEntity, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, NoneCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            var lightBody = new BattleStatusEffect(lightBodyEntity, 1, 1);
            var applyBuffAction = new ApplyEntityStatusEffectBattleAction(target.Player, lightBody);
            context.ActionScheduler.Enqueue(applyBuffAction);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            var observer = new Observer(context, 3);

            context.ActionObserverHub.SubscribeActionModifier<TryUseCardBattleAction>(observer.ModifyTryUseCard);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
        }
    }
}