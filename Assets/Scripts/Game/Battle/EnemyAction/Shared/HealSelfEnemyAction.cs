namespace Battle.Enemies.Actions.Shared
{
    public class HealSelf : EnemyAction
    {
        private int amount;

        public HealSelf(string id, IEnemyBehaviourOwner owner, int amount) : base(id, owner, BattleEnemyActionType.Effect)
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