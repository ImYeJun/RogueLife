using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using Battle.HurtSources;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class OutOfMyWay : CardBattleBehaviour<SingleEnemyCardTarget, SingleEnemyCardTarget>
    {
        private class Observer
        {
            private BattleContext context;
            private BattleHurtSource hurtSource;

            public Observer(BattleContext context, BattleHurtSource hurtSource)
            {
                this.context = context;
                this.hurtSource = hurtSource;
            }

            public void PostEnemyHurt(HurtEnemyBattleAction hurtEnemy, BattleContext context)
            {
                if (hurtEnemy.Source != hurtSource) { return; }

                if (hurtEnemy.Amount > 0 && hurtEnemy.Enemy.CurrentHealth <= 10)
                {
                    var outOfMyWay = new RequestBattleEndBattleAction(BattleResultType.OUT_OF_MY_WAY);
                    context.ActionScheduler.Enqueue(outOfMyWay);
                }

                CleanItself();
            }

            public void OnPlayerTurnEnd(PlayerTurnEndBattleEvent payload)
            {
                CleanItself();
            }

            public void OnBattleEnd(BattleEndBattleEvent payload)
            {
                CleanItself();
            }

            private void CleanItself()
            {
                context.ActionObserverHub.UnsubscribePostObserver<HurtEnemyBattleAction>(PostEnemyHurt);
                context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(OnPlayerTurnEnd);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OutOfMyWay() {}
        private OutOfMyWay(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new OutOfMyWay(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, SingleEnemyCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, SingleEnemyCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 30);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 40);
        }
        private void ExecuteCommonAction(BattleContext context, CardCaster caster, SingleEnemyCardTarget target, int damage)
        {
            var hurtSource = owner.GetAsHurtSource(caster);

            var observer = new Observer(context, hurtSource);
            context.ActionObserverHub.SubscribePostObserver<HurtEnemyBattleAction>(observer.PostEnemyHurt);
            context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(observer.OnPlayerTurnEnd);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);

            var hurtAction = new RequestHurtEntityBattleAction(hurtSource, damage, target.Enemy);
            context.ActionScheduler.Enqueue(hurtAction);
        }
    }
}