using System;
using System.ComponentModel;
using Battle.HurtSources;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class Nani : DisposableBattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Nani() {}
        private Nani(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override void OnApplied()
        {
            context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(HurtOnPlayerTurnEnd);
            context.EventBus.Subscribe<EnemyTurnEndBattleEvent>(HurtOnEnemyTurnEnd);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(HurtOnPlayerTurnEnd);
            context.EventBus.Unsubscribe<EnemyTurnEndBattleEvent>(HurtOnEnemyTurnEnd);
        }

        public void HurtOnPlayerTurnEnd(PlayerTurnEndBattleEvent payload)
        {
            owner.RequestHurt(state.StackCount, new NoneEntitySource());
            RequestExpire();
        }
        public void HurtOnEnemyTurnEnd(EnemyTurnEndBattleEvent payload)
        {
            owner.RequestHurt(state.StackCount, new NoneEntitySource());
            RequestExpire();
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new Nani(context, owner, state);
        }
    }
}