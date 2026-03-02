using Microsoft.SqlServer.Server;

namespace Battle.StartEffects
{
    public class ApplyPlayerStatusEffectEffect : BattleStartEffect
    {
        private BattleStatusEffectEntity statusEffectEntity;
        private int startStack;
        private int startDuration;
        private bool isStatusEffectEthernal;

        public ApplyPlayerStatusEffectEffect(int remainBattleCount, BattleStatusEffectEntity statusEffectEntity, int stack, int duration = -1) 
            : base(remainBattleCount)
        {
            Init(statusEffectEntity, stack, duration);
        }

        public ApplyPlayerStatusEffectEffect(BattleStatusEffectEntity statusEffectEntity, int stack, int duration = -1) 
            : base()
        {
            Init(statusEffectEntity, stack, duration);
        }

        private void Init(BattleStatusEffectEntity statusEffectEntity, int stack, int duration)
        {
            this.statusEffectEntity = statusEffectEntity;
            startStack = stack;
            startDuration = duration;
            isStatusEffectEthernal = duration == -1;
        }

        protected override void OnApplyEffect(BattleContext context)
        {
            var statusEffect = isStatusEffectEthernal ? new BattleStatusEffect(statusEffectEntity, startStack) : new BattleStatusEffect(statusEffectEntity, startStack, startDuration);
            var player = context.PlayerContainer.Player;

            var applyStatueEffectAction = new ApplyEntityStatusEffectBattleAction(player, statusEffect);
            context.ActionScheduler.Enqueue(applyStatueEffectAction);
        }
    }
}