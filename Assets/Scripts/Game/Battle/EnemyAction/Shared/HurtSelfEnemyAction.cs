namespace Battle.Enemies.Actions.Shared
{
    public class HurtSelf : EnemyAction
    {
        private int damage;

        public HurtSelf(string id, IEnemyBehaviourOwner owner, int damage) : base(id, owner, BattleEnemyActionType.Effect)
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