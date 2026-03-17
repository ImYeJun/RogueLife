using NUnit.Framework;

namespace Battle.Enemies.Actions.Shared
{
    public class ApplySelfStatusEffect : EnemyAction
    {
        private BattleStatusEffectEntity statusEffectEntity;
        private int stack;
        private int duration;
        private bool isEthernal;

        public ApplySelfStatusEffect(string id, IEnemyBehaviourOwner owner, BattleStatusEffectEntity statusEffectEntity, int stack, int duration, bool isLastAction = false, bool isOncePerTurn = false) : base(id, owner, isLastAction, isOncePerTurn)
        {
            this.statusEffectEntity = statusEffectEntity;
            this.stack = stack;
            this.duration = duration;
            isEthernal = false;
        }
        public ApplySelfStatusEffect(string id, IEnemyBehaviourOwner owner, BattleStatusEffectEntity statusEffectEntity, int stack, bool isLastAction = false, bool isOncePerTurn = false) : base(id, owner, isLastAction, isOncePerTurn)
        {
            this.statusEffectEntity = statusEffectEntity;
            this.stack = stack;
            duration = int.MaxValue;
            isEthernal = true;
        }

        public override void Execute(BattleContext context)
        {
            var statusEffect = isEthernal ? new BattleStatusEffect(statusEffectEntity, stack) : new BattleStatusEffect(statusEffectEntity, stack, duration);
            var applyStatusEffectAction = new ApplyEntityStatusEffectBattleAction(owner.AsEntity, statusEffect);

            context.ActionScheduler.Enqueue(applyStatusEffectAction);
        }
    }
}