namespace Battle.Enemies.Actions.Shared
{
    public class SpawnEnemy : EnemyAction
    {
        private EnemyEntity entity;
        private int amount;

        public SpawnEnemy(string id, IEnemyBehaviourOwner owner, EnemyEntity entity, int amount = 1) : base(id, owner)
        {
            this.entity = entity;
            this.amount = amount;
        }

        public override void Execute(BattleContext context)
        {
            for (int i = 0; i < amount; i++)
            {
                var enemy = new BattleEnemy(context, entity);
                var spawnEnemyAction = new SpawnEnemyBattleAction(enemy);

                context.ActionScheduler.Enqueue(spawnEnemyAction);
            }
        }
    }
}