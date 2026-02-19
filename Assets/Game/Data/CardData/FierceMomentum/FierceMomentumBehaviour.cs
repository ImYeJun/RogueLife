using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class FierceMomentum : CardBattleBehaviour<PlayerCardTarget, NoneCardTarget>
    {
        [SerializeField] private BattleStatusEffectData lightBodyData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FierceMomentum() {}
        private FierceMomentum(ICardBehaviourOwner owner, BattleStatusEffectData lightBodyData) 
        : base(owner)
        {
            this.lightBodyData = lightBodyData;
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new FierceMomentum(owner, lightBodyData);
        }
        private class ActionModifier
        {
            private int remainObserveCount;
            
            public ActionModifier(IBattleActionObserverHub actionObserverHub)
            {
                remainObserveCount = 3;

                actionObserverHub.SubscribeActionModifier<TryUseCardBattleAction>(ReduceCardActionCost);
            }

            public void ReduceCardActionCost(TryUseCardBattleAction tryUseCard, BattleContext context)
            {
                tryUseCard.ReduceCost(1);
                
                AfterObserve(context);
            }

            private void AfterObserve(BattleContext context)
            {
                if (--remainObserveCount <= 0)
                {
                    context.ActionObserverHub.UnsubscribeActionModifier<TryUseCardBattleAction>(ReduceCardActionCost);
                }
            }
        }

        public override bool IsAbleToUse(BattleContext context, CardTarget target)
        {
            return true;
        }
        public override bool IsAbleToUseReflect(BattleContext context, CardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            var player = target.Player;

            var itsLight = new BattleStatusEffect(lightBodyData, 1, 1);
            var action = new ApplyEntityStatusEffectBattleAction(player, itsLight);
            context.ActionScheduler.Enqueue(action);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            new ActionModifier(context.ActionObserverHub);
        }

    }
}