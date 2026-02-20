using System;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class StoneOfDavid : CardBattleBehaviour<CompositeCardTarget, CompositeCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public StoneOfDavid() {}
        private StoneOfDavid(ICardBehaviourOwner owner) 
        : base(owner) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new StoneOfDavid(owner);
        }

        public override bool OnIsAbleToUse(BattleContext context, CompositeCardTarget target)
        {
            return CheckCommonCondition(target);
        }
        public override bool OnIsAbleToUseReflect(BattleContext context, CompositeCardTarget target)
        {
            return CheckCommonCondition(target);
        }
        private bool CheckCommonCondition(CompositeCardTarget target)
        {
            SingleEnemyCardTarget enemyTarget = GetTarget<SingleEnemyCardTarget>(target);
            PlayerCardTarget playerTarget = GetTarget<PlayerCardTarget>(target);

            return enemyTarget.Enemy.CurrentHealth > playerTarget.Player.CurrentHealth;
        }

        public override void OnDraw(BattleContext context)
        {
            
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, CompositeCardTarget target)
        {
            ExecuteCommonAction(context, target, 0.05);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, CompositeCardTarget target)
        {
            ExecuteCommonAction(context, target, 0.1);
        }
        private void ExecuteCommonAction(BattleContext context, CompositeCardTarget target, double probability)
        {
            if (context.Random.NextDouble() > probability) { return; }

            var enemy = GetTarget<SingleEnemyCardTarget>(target).Enemy;
            var killAction = new KillEntityBattleAction(enemy);
            context.ActionScheduler.Enqueue(killAction);
        }

        private T GetTarget<T>(CompositeCardTarget target) where T : CardTarget
        {
            T result = target.GetTarget<T>();
            if (result is null) { throw new InvalidOperationException("[StoneOfDavid] Seriously? Check the applied target type in the inspector immediately!!!"); }
            return result;
        }
    }
}