using System;
using System.ComponentModel;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class MultiplyingComrades : BattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MultiplyingComrades() {}
        private MultiplyingComrades(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override void OnApplied() { }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            if (!isOwnerDied) return;

            OnExecuted();
            if (owner is BattleEnemy enemy)
            {
                for (int i = 0; i < 2; i++)
                {
                    enemy.Clone(0.5f);
                }
            }
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new MultiplyingComrades(context, owner, state);
        }
    }
}