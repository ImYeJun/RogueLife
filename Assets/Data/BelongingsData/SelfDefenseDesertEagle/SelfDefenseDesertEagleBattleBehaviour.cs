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
            context.EventBus.Subscribe<BattleStartEvent>(PostBattleStart, BattleEventObserverStage.POST);
        }

        protected override void OnRemoved()
        {
            context.EventBus.Unsubscribe<BattleStartEvent>(PostBattleStart);
        }

        public void PostBattleStart(BattleStartEvent payload)
        {
            var enemies = context.EnemySystem.GetBattleEnemies();

            foreach (var enemy in enemies)
            {
                var thatsWeakSpot = new BattleStatusEffect(thatsWeakSpotEntity, 1, 1);
                var applyDebuffAction = new ApplyEntityStatusEffectBattleAction(enemy, thatsWeakSpot);

                context.ActionScheduler.Enqueue(applyDebuffAction);
            }

            Deactivate();
        }
    }
}