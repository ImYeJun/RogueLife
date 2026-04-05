using System;
using System.ComponentModel;
using UnityEngine;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class BattleSelfDefenseDesertEagle : BattleBelongingsBehaviour
    {
        [SerializeField] private BattleStatusEffectEntity thatsWeakSpotEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BattleSelfDefenseDesertEagle() {}
        public BattleSelfDefenseDesertEagle(BattleStatusEffectEntity thatsWeakSpotEntity)
        {
            this.thatsWeakSpotEntity = thatsWeakSpotEntity;
        }

        public override BattleBelongingsBehaviour Clone()
        {
            return new BattleSelfDefenseDesertEagle(thatsWeakSpotEntity);
        }

        protected override void OnApplied()
        {
            context.EventBus.Subscribe<PhaseStartBattleEvent>(PostBattleStart);
        }

        protected override void OnRemoved()
        {
            context.EventBus.Unsubscribe<PhaseStartBattleEvent>(PostBattleStart);
        }

        /* 
         * TODO: The Timing Hack
         * Ideally, this should be observing the "BattleStarted" event as the description says.
         * However, the View (presentation layer) initializes AFTER the "BattleStarted" logic is executed.
         * 
         * For now, to prevent the visual effects from skipping or breaking, the effect is applied 
         * when the FIRST phase starts (PhaseStartBattleEvent) instead.
         * 
         * WARNING: If this timing workaround causes turn logic or status effects to blow up later, 
         * we need to refactor the View initialization architecture and revert this back to "BattleStarted"!
         */
        public void PostBattleStart(PhaseStartBattleEvent payload)
        {
            var enemies = context.EnemySystem.GetBattleEnemies();

            foreach (var enemy in enemies)
            {
                var thatsWeakSpot = new BattleStatusEffect(thatsWeakSpotEntity, 1, 1);
                var applyDebuffAction = new ApplyEntityStatusEffectBattleAction(enemy, thatsWeakSpot);

                OnExecuted();
                context.ActionScheduler.Enqueue(applyDebuffAction);
            }

            Deactivate();
        }
    }
}