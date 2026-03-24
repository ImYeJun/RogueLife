namespace Battle.Enemies.Actions.Shared
{
    public class ApplyPlayerStatusEffect : EnemyAction
    {
        private BattleStatusEffectEntity statusEffectEntity;
        private int stack;
        private int duration;
        private bool isEthernal;

        public ApplyPlayerStatusEffect(string id, IEnemyBehaviourOwner owner, BattleStatusEffectEntity statusEffectEntity, int stack, int duration) : base(id, owner, BattleEnemyActionType.Effect)
        {
            this.statusEffectEntity = statusEffectEntity;
            this.stack = stack;
            this.duration = duration;
            isEthernal = false;
        }
        public ApplyPlayerStatusEffect(string id, IEnemyBehaviourOwner owner, BattleStatusEffectEntity statusEffectEntity, int stack) : base(id, owner, BattleEnemyActionType.Effect)
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