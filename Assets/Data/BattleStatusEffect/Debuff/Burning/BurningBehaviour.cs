using System;
using System.ComponentModel;
using Battle.HurtSources;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class Burning : BattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Burning() {}
        private Burning(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }
        
        public override void OnApplied()
        {
            context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(HurtOnPlayerTurnEnd);
            context.EventBus.Subscribe<EnemyTurnEndBattleEvent>(HurtOnEnemyTurnEnd);
        }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(HurtOnPlayerTurnEnd);
            context.EventBus.Unsubscribe<EnemyTurnEndBattleEvent>(HurtOnEnemyTurnEnd);
        }

        public override void OnMerged() { }

        public void HurtOnPlayerTurnEnd(PlayerTurnEndBattleEvent payload)
        {
            OnExecuted();
            owner.RequestHurt(state.StackCount * 5, new NoneEntitySource());
        }
        public void HurtOnEnemyTurnEnd(EnemyTurnEndBattleEvent payload)
        {
            OnExecuted();
            owner.RequestHurt(state.StackCount * 5, new NoneEntitySource());
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new Burning(context, owner, state);
        }
    }
}