namespace Battle.BattleResultCommands
{
    public class PlayerDiedCommand : BattleResultCommand
    {
        public PlayerDiedCommand(EnemyTier mainEnemyTier) : base(mainEnemyTier, false)
        {
        }

        public override void Resolve(FieldContext context, Node currentNode)
        {
            currentNode.OnPlayerMentalBroken();
        }
    }
}