namespace Battle.Enemies.Actions.Shared
{
    public class RemoveItselfStatusEffect : EnemyAction
    {
        private BattleStatusEffect statusEffect;

        public RemoveItselfStatusEffect(IEnemyBehaviourOwner owner, BattleStatusEffect statusEffect) : base(owner)
        {
            this.statusEffect = statusEffect;
        }

        public override void Execute(BattleContext context)
        {
            var removeStatuEffectAction = new RemoveEntityStatusEffect(owner.AsEntity, statusEffect);

            context.ActionScheduler.Enqueue(removeStatuEffectAction);
        }
    }
}