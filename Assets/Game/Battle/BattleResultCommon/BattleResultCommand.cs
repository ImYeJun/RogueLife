namespace Battle.BattleResultCommands
{
    public abstract class BattleResultCommand
    {
        protected EnemyTier mainEnemyTier;

        protected BattleResultCommand(EnemyTier mainEnemyTier)
        {
            this.mainEnemyTier = mainEnemyTier;
        }

        public abstract void Resolve(FieldContext context, Node currentNode);
    }
}