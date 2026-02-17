using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class SuperHeal : BattleStatusEffectBehaviour, IBattleEventObserver
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SuperHeal() {}
        private SuperHeal(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new SuperHeal(context, owner, state);
        }

        public override void OnApplied()
        {
            context.EventBus.Subscribe(this);
        }

        public void OnBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent is PlayerTurnEndBattleEvent || battleEvent is EnemyTurnEndBattleEvent)
            {
                owner.RequestHeal(state.StackCount * 5);
            }
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe(this);
        }
    }
}