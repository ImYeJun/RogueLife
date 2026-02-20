using System;
using System.ComponentModel;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class NotJustOneHit : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public NotJustOneHit() {}
        private NotJustOneHit(ICardBehaviourOwner owner) 
        : base(owner) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new NotJustOneHit(owner);
        }

        public override bool OnIsAbleToUse(BattleContext context, NoneCardTarget target)
        {
            var hurtEnemies = context.EnemyHistory.HurtEnemies(BattleScope.PHASE);

            return hurtEnemies.Count != 0;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, NoneCardTarget target)
        {
            var hurtEnemies = context.EnemyHistory.HurtEnemies(BattleScope.PHASE);

            return hurtEnemies.Count != 0;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, caster, 20);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            ExecuteCommonAction(context, caster, 25);
        }

        private void ExecuteCommonAction(BattleContext context, CardCaster caster, int damage)
        {
            var hurtEnemies = context.EnemyHistory.HurtEnemies(BattleScope.PHASE);

            foreach (var enemy in hurtEnemies)
            {
                var hurtSource = owner.GetAsHurtSource(caster);
                var action = new RequestHurtEntityBattleAction(hurtSource, damage, enemy);
                context.ActionScheduler.Enqueue(action);
            }
        }
    }
}