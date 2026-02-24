namespace Battle.BattleResultCommands
{
    public class RequestNextNodeSelectionCommand : BattleResultCommand
    {
        public RequestNextNodeSelectionCommand(EnemyTier mainEnemyTier) : base(mainEnemyTier)
        {
        }

        public override void Resolve(FieldContext context, Node currentNode)
        {
            currentNode.RequestNextNodeSelection();
        }
    }
}