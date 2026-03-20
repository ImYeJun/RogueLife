using System.Collections.Generic;

namespace Battle.BattleResultCommands
{
    public class CompositeCommand : BattleResultCommand
    {
        private List<BattleResultCommand> leaves;

        public CompositeCommand(EnemyTier mainEnemyTier, List<BattleResultCommand> leaves, bool hasResolved) : base(mainEnemyTier, hasResolved)
        {
            this.leaves = leaves ?? new List<BattleResultCommand>();
        }

        public override void Resolve(FieldContext context, Node currentNode)
        {
            foreach (var leaf in leaves)
            {
                leaf.Resolve(context, currentNode);
            }
        }
    }
}