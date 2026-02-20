using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class FullyPrepared : CardBattleBehaviour<PlayerCardTarget, PlayerCardTarget>
    {
        private class Observer
        {
            private BattleContext context;
            private PlayerCardTarget target;
            private BattleStatusEffect statusEffect;

            public Observer(BattleContext context, PlayerCardTarget target, BattleStatusEffect statusEffect)
            {
                this.context = context;
                this.target = target;
                this.statusEffect = statusEffect;
            }

            public void OnNextTurnStart(PlayerTurnStartBattleEvent payload)
            {
                var action = new ApplyEntityStatusEffectBattleAction(target.Player, statusEffect);
                context.ActionScheduler.Enqueue(action);

                CleanItself();
            }
            public void OnBattleEnd(BattleEndBattleEvent payload)
            {
                CleanItself();
            }
            public void CleanItself()
            {
                context.EventBus.Unsubscribe<PlayerTurnStartBattleEvent>(OnNextTurnStart);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

        [SerializeField] private BattleStatusEffectData strengthenMuscleData;
        [SerializeField] private BattleStatusEffectData iWillKillYouData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FullyPrepared() {}
        private FullyPrepared(ICardBehaviourOwner owner, BattleStatusEffectData strengthenMuscleData, BattleStatusEffectData iWillKillYouData) 
        : base(owner)
        {
            this.strengthenMuscleData = strengthenMuscleData;
            this.iWillKillYouData = iWillKillYouData;
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new FullyPrepared(owner, strengthenMuscleData, iWillKillYouData);
        }

        public override bool OnIsAbleToUse(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, PlayerCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {

        }

        protected override void OnExecute(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, strengthenMuscleData);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, iWillKillYouData);
        }
        private static void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, BattleStatusEffectData data)
        {
            var statusEffect = new BattleStatusEffect(data, 2, 1);
            var observer = new Observer(context, target, statusEffect);
            context.EventBus.Subscribe<PlayerTurnStartBattleEvent>(observer.OnNextTurnStart);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
        }
    }
}