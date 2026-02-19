using System;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class FierceMomentum : CardBattleBehaviour<PlayerCardTarget, NoneCardTarget>
    {
        [SerializeField] private BattleStatusEffectData lightBodyData;

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

        public override CardBattleBehaviour Clone()
        {
            return new FierceMomentum()
            {
                lightBodyData = this.lightBodyData
            };
        }

        public override bool IsAbleToUse(BattleContext context, CardTarget target)
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