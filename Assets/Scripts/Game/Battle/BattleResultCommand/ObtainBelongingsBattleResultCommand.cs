#nullable enable

namespace Battle.BattleResultCommands
{
    public class ObtainBelongingsCommand : BattleResultCommand
    {
        public ObtainBelongingsCommand(EnemyTier mainEnemyTier) : base(mainEnemyTier, true)
        {
        }

        public override void Resolve(FieldContext context, Node currentNode)
        {
            if (mainEnemyTier != EnemyTier.BOSS) { return; }

            var equippingBelongings = context.BelongingsBag.EquippingBelongings;

            Belongings? rewardingBelongings = context.BelongingsDatabase.GetRandomBelongings(context.Random, equippingBelongings);

            if (rewardingBelongings is null) { return; }
            context.BelongingsBag.TryObtainBelongings(rewardingBelongings);
        }
    }
}