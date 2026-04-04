using System;
using System.ComponentModel;
using Battle.HurtSources;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class DeadlyPoision : BattleStatusEffectBehaviour
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

        public void ReduceHealAmount(HealEntityBattleAction healEntity, BattleContext context)
        {
            if (healEntity.Target != owner) { return; }
            OnExecuted();
            healEntity.Amount /= 2;
        }
        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<HealEntityBattleAction>(ReduceHealAmount, PipelinePhaseStep.EARLY);
            context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(HurtOnPlayerTurnEnd);
            context.EventBus.Subscribe<EnemyTurnEndBattleEvent>(HurtOnEnemyTurnEnd);
        }
        public void HurtOnPlayerTurnEnd(PlayerTurnEndBattleEvent payload)
        {
            OnExecuted();
            owner.TryHurt(state.StackCount * 5, new NoneEntitySource());
        }
        public void HurtOnEnemyTurnEnd(EnemyTurnEndBattleEvent payload)
        {
            OnExecuted();
            owner.TryHurt(state.StackCount * 5, new NoneEntitySource());
        }
        public override void OnMerged() { }
        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<HealEntityBattleAction>(ReduceHealAmount);
            context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(HurtOnPlayerTurnEnd);
            context.EventBus.Unsubscribe<EnemyTurnEndBattleEvent>(HurtOnEnemyTurnEnd);
        }
    }
}