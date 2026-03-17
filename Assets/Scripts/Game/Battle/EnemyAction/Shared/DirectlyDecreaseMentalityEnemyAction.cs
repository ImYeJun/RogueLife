namespace Battle.Enemies.Actions.Shared
{
    public class DirectlyDecreaseMentality : EnemyAction
    {
        private int amount;

        public DirectlyDecreaseMentality(string id, IEnemyBehaviourOwner owner, int amount) : base(id, owner)
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