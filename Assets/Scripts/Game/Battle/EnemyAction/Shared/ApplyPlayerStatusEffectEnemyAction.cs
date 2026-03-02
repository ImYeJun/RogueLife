namespace Battle.Enemies.Actions.Shared
{
    public class ApplyPlayerStatusEffect : EnemyAction
    {
        private BattleStatusEffectEntity statusEffectEntity;
        private int stack;
        private int duration;
        private bool isEthernal;

        public ApplyPlayerStatusEffect(IEnemyBehaviourOwner owner, BattleStatusEffectEntity statusEffectEntity, int stack, int duration) : base(owner)
        {
            this.statusEffectEntity = statusEffectEntity;
            this.stack = stack;
            this.duration = duration;
            isEthernal = false;
        }
        public ApplyPlayerStatusEffect(IEnemyBehaviourOwner owner, BattleStatusEffectEntity statusEffectEntity, int stack) : base(owner)
        {
            this.statusEffectEntity = statusEffectEntity;
            this.stack = stack;
            duration = int.MaxValue;
            isEthernal = true;
        }

        public override void Execute(BattleContext context)
        {
            var statusEffect = isEthernal ? new BattleStatusEffect(statusEffectEntity, stack) : new BattleStatusEffect(statusEffectEntity, stack, duration);
            var applyStatusEffectAction = new ApplyEntityStatusEffectBattleAction(context.PlayerContainer.Player, statusEffect);

            context.ActionScheduler.Enqueue(applyStatusEffectAction);
        }
    }
}