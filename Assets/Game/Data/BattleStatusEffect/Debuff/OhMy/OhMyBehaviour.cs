using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class OhMy : DisposableBattleStatusEffectBehaviour, IBattleActionModifier
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OhMy() {}
        private OhMy(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }
        
        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new OhMy(context, owner, state);
        }

        public void ModifyAction(IBattleAction action, BattleContext context)
        {
            if (action is BattleEntityAction entityAction)
            {
                if (entityAction.Action != owner) { return; }

                entityAction.Nullify();
                RequestExpire();
            }
        }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier(this);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier(this);
        }
    }
}