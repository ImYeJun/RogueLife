using System;
using System.ComponentModel;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class ScrollExplosion : CardBattleBehaviour<SingleEnemyCardTarget, CompositeCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ScrollExplosion() {}
        private ScrollExplosion(ICardBehaviourOwner owner)
        : base(owner) { }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new ScrollExplosion(owner);
        }

        public override bool IsAbleToUse(BattleContext context, CardTarget target)
        {
            return true;
        }

        public override bool IsAbleToUseReflect(BattleContext context, CardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            var targetEnemy = target.Enemy;

            int totalCount = targetEnemy.CurrentBuffs.Count + targetEnemy.CurrentDebuffs.Count;

            int baseDamage = 10;
            var hurtEnemyAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), totalCount * baseDamage, targetEnemy);
            context.ActionScheduler.Enqueue(hurtEnemyAction);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, CompositeCardTarget target)
        {
            var enemyCardTarget = target.GetTarget<SingleEnemyCardTarget>();
            var playerCardTarget = target.GetTarget<PlayerCardTarget>();
            if (enemyCardTarget == null || playerCardTarget == null) { throw new InvalidOperationException("[ScrollExplosion] Seriously? Check the required types in the editor!!"); }

            var targetEnemy = enemyCardTarget.Enemy;
            var player = playerCardTarget.Player;

            int enemyCount = targetEnemy.CurrentBuffs.Count + targetEnemy.CurrentDebuffs.Count;
            int playerCount = player.CurrentBuffs.Count + player.CurrentDebuffs.Count;
            int totalCount = enemyCount + playerCount;

            int baseDamage = 10;
            var hurtEnemyAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), totalCount * baseDamage, targetEnemy);
            context.ActionScheduler.Enqueue(hurtEnemyAction);
        }
    }
}