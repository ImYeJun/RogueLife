using System;
using System.ComponentModel;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class ThatsWeakSpot : DisposableBattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ThatsWeakSpot() {}
        private ThatsWeakSpot(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }
        
        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new ThatsWeakSpot(context, owner, state);
        }

        public void StrengthenDamage(RequestHurtEntityBattleAction requestHurtEntity, BattleContext context)
        {
            if (requestHurtEntity.Target != owner) { return; }

            requestHurtEntity.AddDamage(state.StackCount * 5);
            RequestExpire();
        }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<RequestHurtEntityBattleAction>(StrengthenDamage);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<RequestHurtEntityBattleAction>(StrengthenDamage);
        }
    }
}