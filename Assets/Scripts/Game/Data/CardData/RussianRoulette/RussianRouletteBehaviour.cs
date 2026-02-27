using System;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;
using Battle.HurtSources;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class RussianRoulette : CardBattleBehaviour<CompositeCardTarget, CompositeCardTarget>
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

            public void PostHurtPlayer(HurtPlayerBattleAction hurtPlayer, BattleContext context)
            {
                if (hurtPlayer.Source != hurtSource) { return; }

                if (hurtPlayer.TotalDamage != 0)
                {
                    var requestDrawAction = new RequestDrawCardBattleAction(Guid.NewGuid());
                    context.ActionScheduler.Enqueue(requestDrawAction);
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
                context.ActionObserverHub.UnsubscribePostObserver<HurtPlayerBattleAction>(PostHurtPlayer);
                context.EventBus.Unsubscribe<PlayerTurnEndBattleEvent>(OnPlayerTurnEnd);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RussianRoulette() {}
        private RussianRoulette(ICardBehaviourOwner owner) 
        : base(owner) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new RussianRoulette(owner);
        }

        public override bool OnIsAbleToUse(BattleContext context, CompositeCardTarget target)
        {
            return true;
        }
        public override bool OnIsAbleToUseReflect(BattleContext context, CompositeCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
            
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, CompositeCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, isReflect: false);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, CompositeCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, isReflect: true);
        }

        private void ExecuteCommonAction(BattleContext context, CardCaster caster, CompositeCardTarget target, bool isReflect)
        {
            var playerTarget = target.GetTarget<PlayerCardTarget>();
            var enemyTarget = target.GetTarget<SingleEnemyCardTarget>();

            if (playerTarget is null || enemyTarget is null) 
            { 
                throw new InvalidOperationException("[RussianRoulette] Seriously? Check the required types in the editor!!"); 
            }

            BattleEntity targetEntity = (context.Random.NextDouble() <= 0.5) 
                ? enemyTarget.Enemy 
                : playerTarget.Player;

            var hurtSource = owner.GetAsHurtSource(caster);
            var hurtAction = new RequestHurtEntityBattleAction(hurtSource, 30, targetEntity);
            context.ActionScheduler.Enqueue(hurtAction);

            if (isReflect && targetEntity == playerTarget.Player)
            {
                var observer = new Observer(context, hurtSource);

                context.ActionObserverHub.SubscribePostObserver<HurtPlayerBattleAction>(observer.PostHurtPlayer);
                context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(observer.OnPlayerTurnEnd);
                context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
            }
        }
    }
}