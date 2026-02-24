namespace Battle.Enemies.Actions.Shared
{
    public class SpawnEnemy : EnemyAction
    {
        private EnemyData data;
        private int amount;

        public SpawnEnemy(IEnemyBehaviourOwner owner, EnemyData data, int amount = 1) : base(owner)
        {
            this.data = data;
            this.amount = amount;
        }

        public override void Execute(BattleContext context)
        {
            for (int i = 0; i < amount; i++)
            {
                var enemy = new BattleEnemy(context, data);
                var spawnEnemyAction = new SpawnEnemyBattleAction(enemy);

                context.ActionScheduler.Enqueue(spawnEnemyAction);
            }
        }
    }
}