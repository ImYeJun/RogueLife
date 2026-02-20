using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using Battle.HurtSources;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class TakeThisFirst : CardBattleBehaviour<SingleEnemyCardTarget, SingleEnemyCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TakeThisFirst() {}
        private TakeThisFirst(ICardBehaviourOwner owner) 
        : base(owner) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new TakeThisFirst(owner);
        }

        public override bool OnIsAbleToUse(BattleContext context, SingleEnemyCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, SingleEnemyCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context) { }

        protected override void OnExecute(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            var hurtSource = owner.GetAsHurtSource(caster);
            int damage = 20;
            var targetEntity = target.Enemy;

            var action = new RequestHurtEntityBattleAction(hurtSource, damage, targetEntity);
            context.ActionScheduler.Enqueue(action);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            var hurtSource = owner.GetAsHurtSource(caster);
            int damage = 30;
            var targetEntity = target.Enemy;

            var action = new RequestHurtEntityBattleAction(hurtSource, damage, targetEntity);
            context.ActionScheduler.Enqueue(action);
        }
    }
}