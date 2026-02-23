namespace Battle.Enemies.Actions.Shared
{
    public class HurtItself : EnemyAction
    {
        private int damage;

        public HurtItself(IEnemyBehaviourOwner owner, int damage) : base(owner)
        {
            this.damage = damage;
        }

        public override void Execute(BattleContext context)
        {
            var hurtItselfAction = new RequestHurtEntityBattleAction(owner.AsHurtSource, damage, owner.AsEntity);

            context.ActionScheduler.Enqueue(hurtItselfAction);
        }
    }
}