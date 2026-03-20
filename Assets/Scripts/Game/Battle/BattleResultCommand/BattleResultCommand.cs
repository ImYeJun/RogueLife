namespace Battle.BattleResultCommands
{
    public abstract class BattleResultCommand
    {
        protected EnemyTier mainEnemyTier;
        private bool hasResolved;

        protected BattleResultCommand(EnemyTier mainEnemyTier, bool hasResolved)
        {
            this.mainEnemyTier = mainEnemyTier;
            this.hasResolved = hasResolved;
        }

        public bool HasResolved { get => hasResolved; }

        public abstract void Resolve(FieldContext context, Node currentNode);
    }
}