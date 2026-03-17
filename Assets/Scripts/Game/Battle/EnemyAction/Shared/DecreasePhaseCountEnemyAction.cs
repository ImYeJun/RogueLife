namespace Battle.Enemies.Actions.Shared
{
    public class DecreasePhaseCount : EnemyAction
    {
        int phaseCount;

        public DecreasePhaseCount(string id, IEnemyBehaviourOwner owner, int phaseCount, bool isLastAction = false, bool isOncePerTurn = false) : base(id, owner, isLastAction, isOncePerTurn)
        {
            this.phaseCount = phaseCount;
        }

        public override void Execute(BattleContext context)
        {
            var decreasePhaseCountAction = new DecreasePhaseCountBattleAction(phaseCount);

            context.ActionScheduler.Enqueue(decreasePhaseCountAction);
        }
    }
}