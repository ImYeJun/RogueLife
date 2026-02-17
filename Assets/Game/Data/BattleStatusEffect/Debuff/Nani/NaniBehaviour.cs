using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class Nani : DisposableBattleStatusEffectBehaviour, IBattleEventObserver
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Nani() {}
        private Nani(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override void OnApplied()
        {
            context.EventBus.Subscribe(this);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe(this);
        }

        public void OnBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent is PlayerTurnStartBattleEvent || battleEvent is EnemyTurnStartBattleEvent)
            {
                owner.RequestHurt(state.StackCount, new NoneEntitySource());
                RequestExpire();
            }
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new Nani(context, owner, state);
        }
    }
}