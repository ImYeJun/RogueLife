using System;
using System.ComponentModel;
using System.Resources;
using UnityEngine;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class Bleeding : BattleStatusEffectBehaviour, IBattleEventObserver, IBattleActionPostObserver
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Bleeding() {}
        private Bleeding(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override void OnApplied()
        {
            context.EventBus.Subscribe(this);
            context.ActionObserverHub.SubscribePostObserver(this);
        }

        public override void ActivateEffect()
        {
            owner.RequestHurt(state.StackCount * 5, new NonEntityBattleHurtSource());
        }

        public void BleedAfterAction(BattleContext context)
        {
            owner.RequestHurt(5, new NonEntityBattleHurtSource());
        }
        
        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe(this);
            context.ActionObserverHub.UnsubscribePostObserver(this);
        }

        public void OnBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent is PlayerTurnEndBattleEvent || battleEvent is EnemyTurnEndBattleEvent)
            {
                ActivateEffect();
            }
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
        
        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new Bleeding(context, owner, state);
        }
    }
}