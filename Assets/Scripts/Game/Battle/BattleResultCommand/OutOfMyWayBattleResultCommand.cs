namespace Battle.BattleResultCommands
{
    public class OutOfMyWayCommand : BattleResultCommand
    {
        public OutOfMyWayCommand(EnemyTier mainEnemyTier) : base(mainEnemyTier, false)
        {
        }

        public override void Resolve(FieldContext context, Node currentNode)
        {
            var nextNodes = currentNode.NextNodes;
            var randomNextNode = nextNodes[context.Random.Next(nextNodes.Count)];
        
            currentNode.OnExit(randomNextNode);
        }
    }
}