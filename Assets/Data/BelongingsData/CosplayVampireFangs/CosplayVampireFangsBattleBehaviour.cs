using System;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class BattleCosplayVampireFangs : BattleBelongingsBehaviour
    {
        private bool hasTriggeredThisTurn = false; 

        public override BattleBelongingsBehaviour Clone()
        {
            return new BattleCosplayVampireFangs();
        }

        protected override void OnApplied()
        {
            context.EventBus.Subscribe<EntityHurtBattleEvent>(OnEntityHurt);
            context.EventBus.Subscribe<PlayerTurnStartBattleEvent>(OnPlayerTurnStart); 
        }

        protected override void OnRemoved()
        {
            context.EventBus.Unsubscribe<EntityHurtBattleEvent>(OnEntityHurt);
            context.EventBus.Unsubscribe<PlayerTurnStartBattleEvent>(OnPlayerTurnStart);
        }

        private void OnPlayerTurnStart(PlayerTurnStartBattleEvent payload)
        {
            hasTriggeredThisTurn = false;
        }

        public void OnEntityHurt(EntityHurtBattleEvent payload)
        {
            if (hasTriggeredThisTurn) { return; } 
            
            var player = context.PlayerContainer.Player;
            if (payload.Source.Caster != player) { return; }
            if (payload.Amount <= 0) { return; }

            hasTriggeredThisTurn = true; 

            var healAction = new HealEntityBattleAction(player, 5);

            OnExecuted();
            context.ActionScheduler.Enqueue(healAction);
        }
    }
}