using System;
using System.Linq;

namespace Battle.Enemies.Actions.Shared
{
    public class ClearPlayerStatusEffect : EnemyAction
    {
        private readonly BattleStatusEffectType type;

        public ClearPlayerStatusEffect(string id, IEnemyBehaviourOwner owner, BattleStatusEffectType type = BattleStatusEffectType.ANY) : base(id, owner, BattleEnemyActionType.Effect)
        {
            this.type = type;
        }

        public override void Execute(BattleContext context)
        {
            var player = context.PlayerContainer.Player;
            var playerStatusEffects = player.GetBattleStatusEffects(type);

            foreach (var effect in playerStatusEffects)
            {
                var removeEffectAction = new RemoveEntityStatusEffect(player, effect);

                context.ActionScheduler.Enqueue(removeEffectAction);
            }
        }
    }
}