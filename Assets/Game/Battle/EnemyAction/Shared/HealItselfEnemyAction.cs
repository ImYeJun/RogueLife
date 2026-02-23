namespace Battle.Enemies.Actions.Shared
{
    public class HealItself : EnemyAction
    {
        private int amount;

        public HealItself(IEnemyBehaviourOwner owner, int amount) : base(owner)
        {
            this.amount = amount;
        }

        public override void Execute(BattleContext context)
        {
            var healAction = new HealEntityBattleAction(owner.AsEntity, amount);

            context.ActionScheduler.Enqueue(healAction);
        }
    }
}