namespace Battle.Enemies.Actions.Shared
{
    public class RemovePlayerStatusEffect : EnemyAction
    {
        private BattleStatusEffect statusEffect;

        public RemovePlayerStatusEffect(IEnemyBehaviourOwner owner, BattleStatusEffect statusEffect) : base(owner)
        {
            this.statusEffect = statusEffect;
        }

        public override void Execute(BattleContext context)
        {
            var removeStatuEffectAction = new RemoveEntityStatusEffect(context.PlayerContainer.Player, statusEffect);

            context.ActionScheduler.Enqueue(removeStatuEffectAction);
        }
    }
}