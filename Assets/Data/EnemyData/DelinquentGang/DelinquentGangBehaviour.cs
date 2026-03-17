using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions;
using Battle.Enemies.Actions.Shared;
using Battle.HurtSources;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class DelinquentGang : EliteBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "Enemy_DelinquentGang_Behavior_0";
        private const string SECOND_ACTION = "Enemy_DelinquentGang_Behavior_1";
        private const string THIRD_ACTION = "Enemy_DelinquentGang_Behavior_2";

        [SerializeField] private BattleStatusEffectEntity waterFistEntity;

        private class Imitate : EnemyAction
        {
            public Imitate(IEnemyBehaviourOwner owner, bool isLastAction = false, bool isOncePerTurn = false) : base(owner, isLastAction, isOncePerTurn)
            {
            }

            public override void Execute(BattleContext context)
            {
                var player = context.PlayerContainer.Player;
                var playerBuffs = player.GetBattleStatusEffects(BattleStatusEffectType.BUFF);

                if (playerBuffs.Count <= 0) { return; }

                var selected = playerBuffs[context.Random.Next(playerBuffs.Count)];
                var cloned = new BattleStatusEffect(selected);

                var applyStatusEffectAction = new ApplyEntityStatusEffectBattleAction(owner.AsEntity, cloned);
                context.ActionScheduler.Enqueue(applyStatusEffectAction);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DelinquentGang() {}
        private DelinquentGang(DelinquentGang template, IEnemyBehaviourOwner owner) : base(owner)
        {
            waterFistEntity = template.waterFistEntity;

            availableActions = new Dictionary<string, EnemyAction>
            {
                { FIRST_ACTION, new ApplyPlayerStatusEffect(owner, waterFistEntity, 2, 2) },
                { SECOND_ACTION, new HurtPlayer(owner, 30) },
                { THIRD_ACTION, new Imitate(owner) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ THIRD_ACTION, SECOND_ACTION, SECOND_ACTION, FIRST_ACTION },
                    condition : (context, remainActionCount) => remainActionCount >= 4
                ),
                new Pattern(
                    preset : new List<string> { THIRD_ACTION, THIRD_ACTION },
                    condition : (context, remainActionCount) => context.PlayerContainer.Player.GetBattleStatusEffects(BattleStatusEffectType.BUFF).Count >= 2
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new DelinquentGang(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}