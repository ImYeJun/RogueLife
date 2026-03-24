using System;

namespace Battle.BattleResultCommands
{
    public class ReceiveDamageCommand : BattleResultCommand
    {
        public ReceiveDamageCommand(EnemyTier mainEnemyTier) : base(mainEnemyTier)
        {
        }

        public override void Resolve(FieldContext context, Node currentNode, BattleRewardCollector rewardCollector)
        {
            int damage = mainEnemyTier switch
            {
                EnemyTier.NORMAL => 10,
                EnemyTier.ELITE => 20,
                EnemyTier.BOSS => 35,
                _ => throw new InvalidOperationException($"[ReceiveDamageCommand] {mainEnemyTier} is not supported.")
            };

            context.Health.HurtBattleHealth(damage, isOverflowable : true);
        }
    }
}