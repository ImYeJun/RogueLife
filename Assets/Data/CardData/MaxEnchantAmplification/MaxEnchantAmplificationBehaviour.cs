using System;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;
using UnityEngine.Rendering;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class MaxEnchantAmplification : CardBattleBehaviour<CompositeCardTarget, CompositeCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MaxEnchantAmplification() {}
        private MaxEnchantAmplification(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new MaxEnchantAmplification(owner, targetType, reflectionTargetType);
        }

        public override void OnDraw(BattleContext context)
        {
        }

        public override bool OnIsAbleToUse(BattleContext context, CompositeCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, CompositeCardTarget target)
        {
            return true;
        }

        //* I believe there's no mistake on selecting Card Target in the inspector.
        //* It's a hassle to check the Type...
        protected override void OnExecute(BattleContext context, CardCaster caster, CompositeCardTarget target)
        {
            var player = target.GetTarget<PlayerCardTarget>().Player;
            var playerBuffs = player.CurrentBuffs.Values.ToList();
            for (int i = playerBuffs.Count - 1; i >= 0; i--)
            {
                var owningbuff = playerBuffs[i];
                var clonedBuff = new BattleStatusEffect(owningbuff);
                var buffApplyAction = new ApplyEntityStatusEffectBattleAction(player, clonedBuff);

                context.ActionScheduler.Enqueue(buffApplyAction);
            }

            var allEnemies = target.GetTarget<AllEnemyCardTarget>().Enemies;
            for (int i = allEnemies.Count - 1; i >= 0; i--)
            {  
                var enemy = allEnemies[i];
                var enemyDebuffs = enemy.CurrentDebuffs.Values.ToList();
                for (int j = enemyDebuffs.Count - 1; j >= 0; j--)
                {
                    var owningDebuff = enemyDebuffs[j];
                    var clonedDebuff = new BattleStatusEffect(owningDebuff);
                    var debuffApplyAction = new ApplyEntityStatusEffectBattleAction(enemy, clonedDebuff);

                    context.ActionScheduler.Enqueue(debuffApplyAction);
                }
            }
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, CompositeCardTarget target)
        {
            var player = target.GetTarget<PlayerCardTarget>().Player;
            var allEnemies = target.GetTarget<AllEnemyCardTarget>().Enemies;

            var playerDebuffs = player.CurrentDebuffs.Values.ToList();
            for (int i = playerDebuffs.Count - 1; i >= 0; i--)
            {
                var removeStatusEffectAction = new RemoveEntityStatusEffect(player, playerDebuffs[i]);
                
                context.ActionScheduler.Enqueue(removeStatusEffectAction);
            }
            for (int i = allEnemies.Count - 1; i >= 0; i--)
            {  
                var enemy = allEnemies[i];
                var enemyBuffs = enemy.CurrentBuffs.Values.ToList();
                for (int j = enemyBuffs.Count - 1; j >= 0; j--)
                {
                    var removeStatusEffectAction = new RemoveEntityStatusEffect(enemy, enemyBuffs[j]);

                    context.ActionScheduler.Enqueue(removeStatusEffectAction);
                }
            }

            var playerBuffs = player.CurrentBuffs.Values.ToList();
            for (int i = playerBuffs.Count - 1; i >= 0; i--)
            {
                var owningbuff = playerBuffs[i];
                var clonedBuff = new BattleStatusEffect(owningbuff.Entity, owningbuff.StackCount, owningbuff.RemainTurn * 2);
                var buffApplyAction = new ApplyEntityStatusEffectBattleAction(player, clonedBuff);

                context.ActionScheduler.Enqueue(buffApplyAction);
            }
            for (int i = allEnemies.Count - 1; i >= 0; i--)
            {  
                var enemy = allEnemies[i];
                var enemyDebuffs = enemy.CurrentDebuffs.Values.ToList();
                for (int j = enemyDebuffs.Count - 1; j >= 0; j--)
                {
                    var owningDebuff = enemyDebuffs[j];
                    var clonedDebuff = new BattleStatusEffect(owningDebuff.Entity, owningDebuff.StackCount, owningDebuff.RemainTurn * 2);
                    var debuffApplyAction = new ApplyEntityStatusEffectBattleAction(enemy, clonedDebuff);

                    context.ActionScheduler.Enqueue(debuffApplyAction);
                }
            }
        }
    }
}