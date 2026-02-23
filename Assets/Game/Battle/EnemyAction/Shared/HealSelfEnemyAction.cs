namespace Battle.Enemies.Actions.Shared
{
    public class HealSelf : EnemyAction
    {
        private int amount;

        public HealSelf(IEnemyBehaviourOwner owner, int amount) : base(owner)
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