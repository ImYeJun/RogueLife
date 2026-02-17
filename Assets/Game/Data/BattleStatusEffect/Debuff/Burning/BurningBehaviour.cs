using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class Burning : BattleStatusEffectBehaviour, IBattleEventObserver
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Burning() {}
        private Burning(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }
        
        public override void OnApplied()
        {
            context.EventBus.Subscribe(this);
        }

        public override void ActivateEffect()
        {
            owner.RequestHurt(state.StackCount * 5, new NonEntityBattleHurtSource());
        }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe(this);
        }

        public void OnBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent is PlayerTurnStartBattleEvent || battleEvent is EnemyTurnStartBattleEvent)
            {
                ActivateEffect();
            }
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new Burning(context, owner, state);
        }
    }
}