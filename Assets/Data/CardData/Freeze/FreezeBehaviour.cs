using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class Freeze : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        private class Observer
        {
            private BattleContext context;
            private BattleMaxActionCostModifier costModifier;

            public Observer(BattleContext context, BattleMaxActionCostModifier costModifier)
            {
                this.context = context;
                this.costModifier = costModifier;
            }

            public void OnNextTurnStart(PlayerTurnStartBattleEvent payload)
            {
                var modifyMaxActionCost = new ChangeMaxActionCostBattleAction(costModifier);
                context.ActionScheduler.Enqueue(modifyMaxActionCost);

                CleanItself();
            }

            public void OnBattleEnd(BattleEndBattleEvent payload)
            {
                CleanItself();
            }

            public void CleanItself()
            {
                context.EventBus.Unsubscribe<PlayerTurnStartBattleEvent>(OnNextTurnStart);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

        [SerializeField] private BattleStatusEffectEntity defensiveStanceEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Freeze() {}
        
        private Freeze(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType, BattleStatusEffectEntity defensiveStanceEntity) 
        : base(owner, targetType, reflectionTargetType)
        {
            this.defensiveStanceEntity = defensiveStanceEntity;
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new Freeze(owner, targetType, reflectionTargetType, defensiveStanceEntity);
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
            ExecuteCommonAction(context, target, 2, 20);
        }
        
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            var maxCostModifier = new BattleMaxActionCostModifier(2, BattleScope.TURN);
            var observer = new Observer(context, maxCostModifier);
            context.EventBus.Subscribe<PlayerTurnStartBattleEvent>(observer.OnNextTurnStart);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);

            ExecuteCommonAction(context, target, 3, 30);
        }
        
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, int startTurn, int healAmount)
        {
            var defensiveStance = new BattleStatusEffect(defensiveStanceEntity, 6, startTurn);
            var applyStatusEffectAction = new ApplyEntityStatusEffectBattleAction(target.Player, defensiveStance);
            context.ActionScheduler.Enqueue(applyStatusEffectAction);

            var healAction = new HealEntityBattleAction(target.Player, healAmount);
            context.ActionScheduler.Enqueue(healAction);

            var endTurnAction = new RequestPlayerTurnEndBattleAction();
            context.ActionScheduler.Enqueue(endTurnAction); 
        }
    }
}