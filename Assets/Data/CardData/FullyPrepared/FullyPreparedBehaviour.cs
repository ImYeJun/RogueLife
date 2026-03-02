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

        [SerializeField] private BattleStatusEffectEntity strengthenMuscleEntity;
        [SerializeField] private BattleStatusEffectEntity iWillKillYouEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FullyPrepared() {}
        private FullyPrepared(ICardBehaviourOwner owner, BattleStatusEffectEntity strengthenMuscleEntity, BattleStatusEffectEntity iWillKillYouEntity) 
        : base(owner)
        {
            this.strengthenMuscleEntity = strengthenMuscleEntity;
            this.iWillKillYouEntity = iWillKillYouEntity;
        }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new FullyPrepared(owner, strengthenMuscleEntity, iWillKillYouEntity);
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
            ExecuteCommonAction(context, target, strengthenMuscleEntity);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target, iWillKillYouEntity);
        }
        private static void ExecuteCommonAction(BattleContext context, PlayerCardTarget target, BattleStatusEffectEntity entity)
        {
            var statusEffect = new BattleStatusEffect(entity, 2, 1);
            var observer = new Observer(context, target, statusEffect);
            context.EventBus.Subscribe<PlayerTurnStartBattleEvent>(observer.OnNextTurnStart);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
        }
    }
}