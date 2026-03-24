using System.Collections.Generic;

namespace Battle.BattleResultCommands
{
    public class CompositeCommand : BattleResultCommand
    {
        private List<BattleResultCommand> leaves;

        public CompositeCommand(EnemyTier mainEnemyTier, List<BattleResultCommand> leaves) : base(mainEnemyTier)
        {
            this.leaves = leaves ?? new List<BattleResultCommand>();
        }

        public override void Resolve(FieldContext context, Node currentNode, BattleRewardCollector rewardCollector)
        {
            foreach (var leaf in leaves)
            {
                leaf.Resolve(context, currentNode, rewardCollector);
            }
        }
    }
}