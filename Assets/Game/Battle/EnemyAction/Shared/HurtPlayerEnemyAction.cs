namespace Battle.Enemies.Actions.Shared
{
    public class HurtPlayer : EnemyAction
    {
        private int damage;

        public HurtPlayer(IEnemyBehaviourOwner owner, int damage) : base(owner)
        {
            this.damage = damage;
        }

        public override void Execute(BattleContext context)
        {
            var player = context.PlayerContainer.Player;

            var hurtPlayerAction = new RequestHurtEntityBattleAction(owner.AsHurtSource, damage, player);
            context.ActionScheduler.Enqueue(hurtPlayerAction);
        }
    }
}