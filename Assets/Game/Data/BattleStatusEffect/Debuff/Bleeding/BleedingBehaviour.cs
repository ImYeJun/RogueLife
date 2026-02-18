using System;
using System.ComponentModel;
using System.Resources;
using UnityEngine;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class Bleeding : BattleStatusEffectBehaviour, IBattleActionPostObserver
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Bleeding() {}
        private Bleeding(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override void OnApplied()
        {
            context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(HurtOnPlayerTurnEnd);
            context.EventBus.Subscribe<EnemyTurnEndBattleEvent>(HurtOnEnemyTurnEnd);
            context.ActionObserverHub.SubscribePostObserver(this);
        }
    
        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(HurtOnPlayerTurnEnd);
            context.EventBus.Unsubscribe<EnemyTurnEndBattleEvent>(HurtOnEnemyTurnEnd);
            context.ActionObserverHub.UnsubscribePostObserver(this);
        }

        public override void OnMerged() { }

        public void HurtOnPlayerTurnEnd(PlayerTurnEndBattleEvent payload)
        {
            owner.RequestHurt(state.StackCount * 5, new NoneEntitySource());
        }
        public void HurtOnEnemyTurnEnd(EnemyTurnEndBattleEvent payload)
        {
            owner.RequestHurt(state.StackCount * 5, new NoneEntitySource());
        }

        public void PostObserveAction(IBattleAction action, BattleContext context)
        {
            if (action is BattleEntityAction battleEntityAction)
            {
                if (battleEntityAction.IsNullified) return;

                if (battleEntityAction.Actor == owner)
                {
                    battleEntityAction.AddActionOnScopeClose(BleedAfterAction);
                }
            }
        }
        
        public void BleedAfterAction(BattleContext context)
        {
            owner.RequestHurt(5, new NoneEntitySource());
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new Bleeding(context, owner, state);
        }
    }
}