using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class DontTouch : DisposableBattleStatusEffectBehaviour, IBattleActionModifier
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DontTouch() {}
        private DontTouch(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier(this);
        }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier(this);
        }

        public override void PerformAction() { }

        public void ModifyAction(IBattleAction action, BattleContext context)
        {
            if (action is not BattleEntityAction entityAction) return;
            if (entityAction.Actor == owner) return; 
            
            if (entityAction is IEntityTargetedBattleAction targetedAction)
            {
                if (targetedAction.Target == owner)
                {
                    BlockAndConsume(entityAction);
                }
            }
        }

        private void BlockAndConsume(BattleEntityAction action)
        {
            action.Nullify();
            
            ActivateEffect();
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new DontTouch(context, owner, state);
        }
    }
}