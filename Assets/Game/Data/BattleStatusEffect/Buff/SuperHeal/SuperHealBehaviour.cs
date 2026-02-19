using System;
using System.ComponentModel;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class SuperHeal : BattleStatusEffectBehaviour
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
            context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(HealOnPlayerTurnEnd);
            context.EventBus.Subscribe<EnemyTurnEndBattleEvent>(HealOnEnemyTurnEnd);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(HealOnPlayerTurnEnd);
            context.EventBus.Unsubscribe<EnemyTurnEndBattleEvent>(HealOnEnemyTurnEnd);
        }

        public void HealOnPlayerTurnEnd(PlayerTurnEndBattleEvent payload)
        {
            owner.RequestHeal(state.StackCount * 5);
        }
        public void HealOnEnemyTurnEnd(EnemyTurnEndBattleEvent payload)
        {
            owner.RequestHeal(state.StackCount * 5);
        }
    }
}