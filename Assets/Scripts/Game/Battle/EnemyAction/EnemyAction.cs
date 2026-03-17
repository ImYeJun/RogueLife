namespace Battle.Enemies.Actions
{
    public abstract class EnemyAction
    {
        private bool isLastAction;
        private bool isOncePerTurn;
        private string id;
        protected IEnemyBehaviourOwner owner;

        public bool IsLastAction { get => isLastAction; }
        public bool IsOncePerTurn { get => isOncePerTurn; }
        public string Id => id;

        protected EnemyAction(string id, IEnemyBehaviourOwner owner, bool isLastAction = false, bool isOncePerTurn = false)
        {
            this.id = id;
            this.owner = owner;
            this.isLastAction = isLastAction;
            this.isOncePerTurn = isOncePerTurn;
        }

        public abstract void Execute(BattleContext context);
    }
}