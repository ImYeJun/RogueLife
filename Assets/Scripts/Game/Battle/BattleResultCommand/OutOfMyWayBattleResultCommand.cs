namespace Battle.BattleResultCommands
{
    public class OutOfMyWayCommand : BattleResultCommand
    {
        public OutOfMyWayCommand(EnemyTier mainEnemyTier) : base(mainEnemyTier)
        {
        }

        public override void Resolve(FieldContext context, Node currentNode, BattleRewardCollector rewardCollector)
        {
            var nextNodes = currentNode.NextNodes;
            var randomNextNode = nextNodes[context.Random.Next(nextNodes.Count)];
        
            currentNode.OnExit(randomNextNode);
        }
    }
}