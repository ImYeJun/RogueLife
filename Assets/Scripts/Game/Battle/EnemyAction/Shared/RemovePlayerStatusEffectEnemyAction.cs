using System.Linq;

namespace Battle.Enemies.Actions.Shared
{
    public class RemovePlayerStatusEffect : EnemyAction
    {
        private readonly BattleStatusEffectType type;
        private int amount;

        public RemovePlayerStatusEffect(string id, IEnemyBehaviourOwner owner, BattleStatusEffectType type, int amount = 1) : base(id, owner, BattleEnemyActionType.Effect)
        {
            this.amount = amount;
            this.type = type;
        }

        public override void Execute(BattleContext context)
        {
            var player = context.PlayerContainer.Player;
            var playerStatusEffects = player.GetBattleStatusEffects(type);

            if (playerStatusEffects.Count <= 0) { return; }

            var random = context.Random;
            var selectedStatusEffects = playerStatusEffects.OrderBy(sel => random.Next()).Take(amount);

            foreach (var effect in selectedStatusEffects)
            {
                var removeEffectAction = new RemoveEntityStatusEffect(player, effect);

                context.ActionScheduler.Enqueue(removeEffectAction);
            }
        }
    }
}