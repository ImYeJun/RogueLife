using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class OhMy : DisposableBattleStatusEffectBehaviour
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

        public void NullifyAction(BattleEntityAction entityAction, BattleContext context)
        {
            if (entityAction.Action != owner) { return; }

            entityAction.Nullify();
            RequestExpire();
        }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<BattleEntityAction>(NullifyAction);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<BattleEntityAction>(NullifyAction);
        }
    }
}