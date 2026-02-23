namespace Battle.Enemies.Actions.Shared
{
    public class SpawnEnemy : EnemyAction
    {
        private BattleEnemy enemy;

        public SpawnEnemy(IEnemyBehaviourOwner owner, BattleEnemy enemy) : base(owner)
        {
            this.enemy = enemy;
        }

        public override void Execute(BattleContext context)
        {
            var spawnEnemyAction = new SpawnEnemyBattleAction(enemy);

            context.ActionScheduler.Enqueue(spawnEnemyAction);
        }
    }
}