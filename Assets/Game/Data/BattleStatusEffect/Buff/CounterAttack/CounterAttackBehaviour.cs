using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class CounterAttack : DisposableBattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CounterAttack() {}
        private CounterAttack(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override void OnApplied()
        {
            context.EventBus.Subscribe<EntityHurtBattleEvent>(ExecuteCounterAttack);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe<EntityHurtBattleEvent>(ExecuteCounterAttack);
        }

        public void ExecuteCounterAttack(EntityHurtBattleEvent entityHurtBattleEvent)
        {
            var victim = entityHurtBattleEvent.Victim;
            if (victim != owner) { return; }

            var source = entityHurtBattleEvent.Source;
            if (source.Caster is not BattleEntity sourceEntity) { return; }
            
            context.ActionScheduler.Enqueue(new RequestHurtEntityBattleAction(owner.GetAsHurtSource(), state.StackCount * 5, sourceEntity));
            RequestExpire();
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new CounterAttack(context, owner, state);
        }
    }
}