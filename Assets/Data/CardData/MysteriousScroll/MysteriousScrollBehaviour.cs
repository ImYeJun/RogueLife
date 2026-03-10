using System;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;
using UnityEngine;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class MysteriousScroll : CardBattleBehaviour<PlayerCardTarget, CompositeCardTarget>
    {

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MysteriousScroll() {}
        private MysteriousScroll(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType)
        : base(owner, targetType, reflectionTargetType) { }
        
        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new MysteriousScroll(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, PlayerCardTarget target)
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

        protected override void OnExecute(BattleContext context, CardCaster caster, PlayerCardTarget target)
        {
            ExecuteCommonAction(context, target);
        }
        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, CompositeCardTarget target)
        {
            var playerTarget = target.GetTarget<PlayerCardTarget>();
            ExecuteCommonAction(context, playerTarget);

            var enemyTarget = target.GetTarget<SingleEnemyCardTarget>();
            var enemy = enemyTarget.Enemy;

            var randomDebuffData = context.BattleStatusEffectDatabase.GetRandomData(context.Random, BattleStatusEffectType.DEBUFF);

            if (randomDebuffData is null) { return; }

            var randomDebuff = new BattleStatusEffect(randomDebuffData, 2, 3);
            var applyDebuffAction = new ApplyEntityStatusEffectBattleAction(enemy, randomDebuff);

            context.ActionScheduler.Enqueue(applyDebuffAction);
        }
        private void ExecuteCommonAction(BattleContext context, PlayerCardTarget playerTarget)
        {
            var player = playerTarget.Player;
            var randomBuffData = context.BattleStatusEffectDatabase.GetRandomData(context.Random, BattleStatusEffectType.BUFF);

            if (randomBuffData is null) { return; }

            var randomBuff = new BattleStatusEffect(randomBuffData, 2, 3);
            var applyBuffAction = new ApplyEntityStatusEffectBattleAction(player, randomBuff);

            context.ActionScheduler.Enqueue(applyBuffAction);
        }
    }
}