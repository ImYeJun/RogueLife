using System;
using System.ComponentModel;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class NanoMachine : DisposableBattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public NanoMachine() {}
        private NanoMachine(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }
        
        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new NanoMachine(context, owner, state);
        }

        public override void OnApplied()
        {
            context.EventBus.Subscribe<EntityHurtBattleEvent>(PleaseHealMyOwner);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe<EntityHurtBattleEvent>(PleaseHealMyOwner);
        }

        public void PleaseHealMyOwner(EntityHurtBattleEvent payload)
        {
            if (payload.Victim != owner) { return; }

            int healAmount = (int)(payload.Amount * 0.5);
            owner.RequestHeal(healAmount);
            RequestExpire();
        }
    }
}