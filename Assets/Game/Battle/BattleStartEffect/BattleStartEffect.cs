using System;

namespace Battle.StartEffects
{
    public abstract class BattleStartEffect
    {
        protected int remainBattleCount;
        protected bool isEternal;

        protected BattleStartEffect(int remainBattleCount)
        {
            this.remainBattleCount = remainBattleCount;
            isEternal = false;
        }
        protected BattleStartEffect()
        {
            remainBattleCount = int.MaxValue;
            isEternal = true;
        }

        public bool IsExpired => isEternal ? false : remainBattleCount <= 0;

        public void ApplyEffect(BattleContext context)
        {
            OnApplyEffect(context);

            if (isEternal) { return; }

            remainBattleCount--;
        }

        protected abstract void OnApplyEffect(BattleContext context);
    }
}