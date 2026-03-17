using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class StrangePuddle : EliteBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "Enemy_StrangePuddle_Behavior_0";
        private const string SECOND_ACTION = "Enemy_StrangePuddle_Behavior_1";
        private const string THIRD_ACTION = "Enemy_StrangePuddle_Behavior_2";

        [SerializeField] private BattleStatusEffectEntity deadlyPoisionEntity;
        [SerializeField] private BattleStatusEffectEntity heavyBodyEntity;

        private class CorruptPlayerStatus : EnemyAction
        {
            public CorruptPlayerStatus(string id, IEnemyBehaviourOwner owner) : base(id, owner)
            {
            }

            public override void Execute(BattleContext context)
            {
                var player = context.PlayerContainer.Player;
                var ownerAsEntity = owner.AsEntity;
                
                var playerBuffs = player.GetBattleStatusEffects(BattleStatusEffectType.BUFF);
                if (playerBuffs.Count >= 1)
                {
                    var selectedBuff = playerBuffs[context.Random.Next(playerBuffs.Count)];

                    var removeBuffAction = new RemoveEntityStatusEffect(player, selectedBuff);
                    context.ActionScheduler.Enqueue(removeBuffAction);

                    var applyBuffAction = new ApplyEntityStatusEffectBattleAction(ownerAsEntity, selectedBuff);
                    context.ActionScheduler.Enqueue(applyBuffAction);
                }

                var playerDebuffs = player.GetBattleStatusEffects(BattleStatusEffectType.DEBUFF);
                foreach (var debuff in playerDebuffs)
                {
                    var enhancedDebuff = new BattleStatusEffect(debuff.Entity, 1, debuff.RemainTurn + 1);

                    var applyDebuffAction = new ApplyEntityStatusEffectBattleAction(player, enhancedDebuff);
                    context.ActionScheduler.Enqueue(applyDebuffAction);
                }
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public StrangePuddle() {}
        private StrangePuddle(StrangePuddle template, IEnemyBehaviourOwner owner) : base(owner)
        {
            deadlyPoisionEntity = template.deadlyPoisionEntity;
            heavyBodyEntity = template.heavyBodyEntity;

            availableActions = new Dictionary<string, EnemyAction>
            {
                { FIRST_ACTION, new ApplyPlayerStatusEffect(FIRST_ACTION, owner, deadlyPoisionEntity, 2, 3) },
                { SECOND_ACTION, new ApplyPlayerStatusEffect(SECOND_ACTION, owner, heavyBodyEntity, 2, 2) },
                { THIRD_ACTION, new CorruptPlayerStatus(THIRD_ACTION, owner) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ FIRST_ACTION, FIRST_ACTION, SECOND_ACTION },
                    condition : (context, remainActionCount) => remainActionCount >= 4
                ),
                new Pattern(
                    preset : new List<string> { SECOND_ACTION, THIRD_ACTION },
                    condition : (context, remainActionCount) => context.PlayerContainer.Player.GetBattleStatusEffects(BattleStatusEffectType.BUFF).Count > 0
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new StrangePuddle(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}