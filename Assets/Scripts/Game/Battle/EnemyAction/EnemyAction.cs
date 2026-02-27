namespace Battle.Enemies.Actions
{
    public abstract class EnemyAction
    {
        private bool isLastAction;
        private bool isOncePerTurn;
        protected IEnemyBehaviourOwner owner;

        public bool IsLastAction { get => isLastAction; }
        public bool IsOncePerTurn { get => isOncePerTurn; }

        protected EnemyAction(IEnemyBehaviourOwner owner, bool isLastAction = false, bool isOncePerTurn = false)
        {
            this.owner = owner;
            this.isLastAction = isLastAction;
            this.isOncePerTurn = isOncePerTurn;
        }

        public abstract void Execute(BattleContext context);
    }
}