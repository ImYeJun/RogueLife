using System;
using System.ComponentModel;
using UnityEngine;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class BattleBasicImplant : BattleBelongingsBehaviour
    {
        [SerializeField] BattleStatusEffectData stunnedData;
        private int currentProbability = 0;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BattleBasicImplant() {}
        public BattleBasicImplant(BattleStatusEffectData stunnedData)
        {
            this.stunnedData = stunnedData;
        }

        public override BattleBelongingsBehaviour Clone()
        {
            return new BattleBasicImplant(stunnedData);
        }

        protected override void OnApplied()
        {
            context.ActionObserverHub.SubscribePreObserver<UseCardBattleAction>(PreUseCard);
        }

        protected override void OnRemoved()
        {
            context.ActionObserverHub.UnsubscribePreObserver<UseCardBattleAction>(PreUseCard);
        }

        public void PreUseCard(UseCardBattleAction useCard, BattleContext context)
        {
            currentProbability += currentProbability == 0 ? 5 : 4;

            if (context.Random.Next(100) > currentProbability) { return; }

            var stunned = new BattleStatusEffect(stunnedData, 1, 1);
            var applyDebuffAction = new ApplyEntityStatusEffectBattleAction(context.PlayerContainer.Player, stunned);

            context.ActionScheduler.Enqueue(applyDebuffAction);
        }
    }
}