namespace Battle.Enemies.Actions.Shared
{
    public class DirectlyDecreaseMentality : EnemyAction
    {
        private int amount;

        public DirectlyDecreaseMentality(IEnemyBehaviourOwner owner, int amount) : base(owner)
        {
            this.amount = amount;
        }

        public override void Execute(BattleContext context)
        {
            var decreaseMentalityAction = new DirectlyDecreaseMentalityBattleAction(amount);

            context.ActionScheduler.Enqueue(decreaseMentalityAction);
        }
    }
}