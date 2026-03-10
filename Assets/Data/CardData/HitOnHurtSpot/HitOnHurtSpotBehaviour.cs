using System;
using System.ComponentModel;
using System.Runtime.Remoting.Contexts;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class HitOnHurtSpot : CardBattleBehaviour<SingleEnemyCardTarget, SingleEnemyCardTarget>
    {
        private class Observer
        {
            private BattleContext context;
            private int additionalDamage;
            private RequestHurtEntityBattleAction targetAction;

            public Observer(BattleContext context, int additionalDamage, RequestHurtEntityBattleAction targetAction)
            {
                this.context = context;
                this.additionalDamage = additionalDamage;
                this.targetAction = targetAction;
            }

            public void ModifyAction(RequestHurtEntityBattleAction action, BattleContext context)
            {
                if (targetAction != action) { return; }

                if (!action.Target.IsFullHealth)
                {
                    action.AddDamage(additionalDamage);
                }

                CleanItself();
            }

            public void OnBattleEnd(BattleEndBattleEvent payload)
            {
                CleanItself();
            }

            public void CleanItself()
            {
                context.ActionObserverHub.UnsubscribeActionModifier<RequestHurtEntityBattleAction>(ModifyAction);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HitOnHurtSpot() {}
        private HitOnHurtSpot(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new HitOnHurtSpot(owner, targetType, reflectionTargetType);
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
            ExecuteCommonAction(context, caster, target, 20);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            ExecuteCommonAction(context, caster, target, 25);
        }
        private void ExecuteCommonAction(BattleContext context, CardCaster caster, SingleEnemyCardTarget target, int damage)
        {
            var enemy = target.Enemy;

            var hurtAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), damage, enemy);

            var observer = new Observer(context, damage, hurtAction);
            context.ActionObserverHub.SubscribeActionModifier<RequestHurtEntityBattleAction>(observer.ModifyAction);
            context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);

            context.ActionScheduler.Enqueue(hurtAction);
        }
    }
}