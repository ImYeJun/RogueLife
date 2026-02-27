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
            private PlayerCardTarget target;

            public Observer(BattleContext context, PlayerCardTarget target)
            {
                this.context = context;
                this.target = target;
            }

            public void NullifyAction(BattleEntityAction entityAction, BattleContext context)
            {
                if (entityAction.Actor == target.Player) { return; }

                if (entityAction.Action is not IEntityTargetedBattleAction targetedBattleAction) { return; }
                if (targetedBattleAction.Target == target.Player)
                {
                    entityAction.Nullify();
                }
            }
            public void OnNextTurnStart(PlayerTurnStartBattleEvent payload)
            {
                CleanItself();
            } 
            public void OnBattleEnd(BattleEndBattleEvent payload)
            {
                CleanItself();
            }
            public void CleanItself()
            {
                context.ActionObserverHub.UnsubscribeActionModifier<BattleEntityAction>(NullifyAction);
                context.EventBus.Unsubscribe<PlayerTurnStartBattleEvent>(OnNextTurnStart);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Freeze() {}
        private Freeze(ICardBehaviourOwner owner) 
        : base(owner)
        {
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new Freeze(owner);
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
            ExecuteCommonAction(context, target, true);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, bool isHealable = false)
        {
            var observer = new Observer(context, target);
            context.ActionObserverHub.SubscribeActionModifier<BattleEntityAction>(observer.NullifyAction);
            context.EventBus.Subscribe<PlayerTurnStartBattleEvent>(observer.OnNextTurnStart);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
            
            if (isHealable)
            {
                var healAction = new HealEntityBattleAction(target.Player, 30);
                context.ActionScheduler.Enqueue(healAction);
            }

            var endTurnAction = new RequestPlayerTurnEndBattleAction();
            context.ActionScheduler.Enqueue(endTurnAction); 
        }
    }
}