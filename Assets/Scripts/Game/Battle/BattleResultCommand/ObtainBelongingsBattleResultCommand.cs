#nullable enable

namespace Battle.BattleResultCommands
{
    public class ObtainBelongingsCommand : BattleResultCommand
    {
        public ObtainBelongingsCommand(EnemyTier mainEnemyTier) : base(mainEnemyTier)
        {
        }

        public override void Resolve(FieldContext context, Node currentNode, BattleRewardCollector rewardCollector)
        {
            if (mainEnemyTier != EnemyTier.BOSS) { return; }

            var equippingBelongings = context.BelongingsBag.EquippingBelongings;

            Belongings? rewardingBelongings = context.BelongingsDatabase.GetRandomBelongings(context.Random, equippingBelongings);

            if (rewardingBelongings is null) { return; }
            var reward = new BelongingsBattleReward(rewardingBelongings);
            rewardCollector.AddCandidate(reward);
        }
    }
}