using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class DeadlyPoision : BattleStatusEffectBehaviour, IBattleActionModifier
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
            context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(HurtOnPlayerTurnEnd);
            context.EventBus.Subscribe<EnemyTurnEndBattleEvent>(HurtOnEnemyTurnEnd);
        }

        public void HurtOnPlayerTurnEnd(PlayerTurnEndBattleEvent payload)
        {
            owner.RequestHurt(state.StackCount * 5, new NoneEntitySource());
        }
        public void HurtOnEnemyTurnEnd(EnemyTurnEndBattleEvent payload)
        {
            owner.RequestHurt(state.StackCount * 5, new NoneEntitySource());
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(HurtOnPlayerTurnEnd);
            context.EventBus.Unsubscribe<EnemyTurnEndBattleEvent>(HurtOnEnemyTurnEnd);
        }
    }
}