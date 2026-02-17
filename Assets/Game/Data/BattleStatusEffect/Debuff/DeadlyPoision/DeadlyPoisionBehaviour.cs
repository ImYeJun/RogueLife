using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class DeadlyPoision : BattleStatusEffectBehaviour, IBattleEventObserver, IBattleActionModifier
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DeadlyPoision() {}
        private DeadlyPoision(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new DeadlyPoision(context, owner, state);
        }

        public void ModifyAction(IBattleAction action, BattleContext context)
        {
            if (action is HealEntityBattleAction healEntity)
            {
                if (healEntity.Target != owner) { return; }
                healEntity.Amount /= 2;
            }
        }

        public override void OnApplied()
        {
            context.EventBus.Subscribe(this);
        }

        public void OnBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent is PlayerTurnEndBattleEvent || battleEvent is EnemyTurnEndBattleEvent)
            {
                owner.RequestHurt(state.StackCount * 5, new NoneEntitySource());
            }
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe(this);
        }
    }
}