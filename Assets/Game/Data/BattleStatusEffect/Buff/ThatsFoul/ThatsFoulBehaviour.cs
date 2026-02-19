using System;
using System.ComponentModel;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class ThatsFoul : DisposableBattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ThatsFoul() {}
        private ThatsFoul(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }
        
        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new ThatsFoul(context, owner, state);
        }
        
        //* Fxxcked Name. What a shit!
        public void NullifyOnTargetedToHurt(RequestHurtEntityBattleAction requestHurtEntity, BattleContext context)
        {
            if (requestHurtEntity.Target != owner) { return; }

            requestHurtEntity.Nullify();
            RequestExpire();
        }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<RequestHurtEntityBattleAction>(NullifyOnTargetedToHurt);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<RequestHurtEntityBattleAction>(NullifyOnTargetedToHurt);
        }
    }
}