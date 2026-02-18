#nullable enable

using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class HolyShield : DisposableBattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HolyShield() {}
        private HolyShield(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new HolyShield(context, owner, state);
        }

        public override void OnApplied()
        {
            context.EventBus.Subscribe<EntityHurtBattleEvent>(ReflectDamage);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe<EntityHurtBattleEvent>(ReflectDamage);
        }

        public void ReflectDamage(EntityHurtBattleEvent payload)
        {
            if (payload.Victim != owner) { return; }

            BattleEntity? sourceEntitiy = payload.Source.Caster;
            if (sourceEntitiy is null) { return; }

            int damage = (int)(payload.Amount * 0.5);
            var action = new RequestHurtEntityBattleAction(new NoneEntitySource(), damage, sourceEntitiy);
            context.ActionScheduler.Enqueue(action);
            RequestExpire();
        }
    }
}