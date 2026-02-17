using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class HeavyBody : DisposableBattleStatusEffectBehaviour, IBattleActionModifier
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HeavyBody() {}
        private HeavyBody(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new HeavyBody(context, owner, state);
        }

        public void ModifyAction(IBattleAction action, BattleContext context)
        {
            if (action is TryUseCardBattleAction tryUseCard)
            {
                tryUseCard.IncreaseCost(state.StackCount);

                RequestExpire();
            }
        }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier(this);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier(this);
        }
    }
}