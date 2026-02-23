namespace Battle.Enemies.Actions
{
    public abstract class EnemyAction
    {
        protected IEnemyBehaviourOwner owner;

        protected EnemyAction(IEnemyBehaviourOwner owner)
        {
            this.owner = owner;
        }

        public abstract void Execute(BattleContext context);
    }
}