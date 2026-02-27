using Microsoft.SqlServer.Server;

namespace Battle.StartEffects
{
    public class ApplyPlayerStatusEffectEffect : BattleStartEffect
    {
        private BattleStatusEffectData statusEffectData;
        private int startStack;
        private int startDuration;
        private bool isStatusEffectEthernal;

        public ApplyPlayerStatusEffectEffect(int remainBattleCount, BattleStatusEffectData data, int stack, int duration = -1) 
            : base(remainBattleCount)
        {
            Init(data, stack, duration);
        }

        public ApplyPlayerStatusEffectEffect(BattleStatusEffectData data, int stack, int duration = -1) 
            : base()
        {
            Init(data, stack, duration);
        }

        private void Init(BattleStatusEffectData data, int stack, int duration)
        {
            statusEffectData = data;
            startStack = stack;
            startDuration = duration;
            isStatusEffectEthernal = duration == -1;
        }

        protected override void OnApplyEffect(BattleContext context)
        {
            var statusEffect = isStatusEffectEthernal ? new BattleStatusEffect(statusEffectData, startStack) : new BattleStatusEffect(statusEffectData, startStack, startDuration);
            var player = context.PlayerContainer.Player;

            var applyStatueEffectAction = new ApplyEntityStatusEffectBattleAction(player, statusEffect);
            context.ActionScheduler.Enqueue(applyStatueEffectAction);
        }
    }
}