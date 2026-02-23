namespace Battle.Enemies.Actions.Shared
{
    public class ApplySelfStatusEffect : EnemyAction
    {
        private BattleStatusEffectData statusEffectData;
        private int stack;
        private int duration;
        private bool isEthernal;

        public ApplySelfStatusEffect(IEnemyBehaviourOwner owner, BattleStatusEffectData statusEffectData, int stack, int duration) : base(owner)
        {
            this.statusEffectData = statusEffectData;
            this.stack = stack;
            this.duration = duration;
            isEthernal = false;
        }
        public ApplySelfStatusEffect(IEnemyBehaviourOwner owner, BattleStatusEffectData statusEffectData, int stack) : base(owner)
        {
            this.statusEffectData = statusEffectData;
            this.stack = stack;
            duration = int.MaxValue;
            isEthernal = true;
        }

        public override void Execute(BattleContext context)
        {
            var statusEffect = isEthernal ? new BattleStatusEffect(statusEffectData, stack) : new BattleStatusEffect(statusEffectData, stack, duration);
            var applyStatusEffectAction = new ApplyEntityStatusEffectBattleAction(owner.AsEntity, statusEffect);

            context.ActionScheduler.Enqueue(applyStatusEffectAction);
        }
    }
}