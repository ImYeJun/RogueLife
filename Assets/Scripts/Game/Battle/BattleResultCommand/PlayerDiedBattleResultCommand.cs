namespace Battle.BattleResultCommands
{
    public class PlayerDiedCommand : BattleResultCommand
    {
        public PlayerDiedCommand(EnemyTier mainEnemyTier) : base(mainEnemyTier)
        {
        }

        public override void Resolve(FieldContext context, Node currentNode, BattleRewardCollector rewardCollector)
        {
            currentNode.OnPlayerMentalBroken();
        }
    }
}