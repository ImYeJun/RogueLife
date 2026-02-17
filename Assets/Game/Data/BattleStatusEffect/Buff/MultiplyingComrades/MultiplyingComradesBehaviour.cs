using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
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

            if (owner is ICloneableBattleEntity origin)
            {
                for (int i = 0; i < 2; i++)
                {
                    //* Hard Casting since the given BattleStatusEffect is guaranteed that it's only used in "Enemy" trait entity (Battle Enemy).
                    //* But if new feature implements entity whose trait is also "Enemy, This code shall be refactored.
                    var clone = origin.Clone(0.5f) as BattleEnemy;
                    if (clone == null) return;
                    context.EnemySystem.SpawnEnemy(clone);
                }
            }
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new MultiplyingComrades(context, owner, state);
        }
    }
}