using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class UnknownEntity : BossBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "first";
        private const string SECOND_ACTION = "second";
        private const string THIRD_ACTION = "third";
        private const string FOURTH_ACTION = "fourth";
        private const string FIFTH_ACTION = "fifth";

        [SerializeField] private BattleStatusEffectEntity burningEntity;
        [SerializeField] private BattleStatusEffectEntity deadlyPoisionEntity;
        [SerializeField] private BattleStatusEffectEntity bleedingEntity;

        private class Payback : EnemyAction
        {
            public Payback(IEnemyBehaviourOwner owner, bool isLastAction = false) : base(owner, isLastAction)
            {
            }

            public override void Execute(BattleContext context)
            {
                var executeEffectCounts = context.BattleDeckHistory.GetExecuteCardEffectCount(BattleScope.TURN);
                int damage = executeEffectCounts * 10;

                var hurtPlayerAction = new RequestHurtEntityBattleAction(owner.AsHurtSource, damage, context.PlayerContainer.Player);
                context.ActionScheduler.Enqueue(hurtPlayerAction);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public UnknownEntity() {}
        private UnknownEntity(UnknownEntity template, IEnemyBehaviourOwner owner) : base(owner)
        {
            burningEntity = template.burningEntity;
            deadlyPoisionEntity = template.deadlyPoisionEntity;
            bleedingEntity = template.bleedingEntity;

            availableActions = new Dictionary<string, EnemyAction>
            {
                { FIRST_ACTION, new HurtPlayer(owner, 50) },
                { SECOND_ACTION, new CompositeEnemyAction(owner, new List<EnemyAction>()
                {
                    new RemovePlayerStatusEffect(owner, BattleStatusEffectType.BUFF),
                    new ClearSelfStatusEffect(owner, BattleStatusEffectType.DEBUFF)
                }) },
                { THIRD_ACTION, new DumpPlayerHandCard(owner, 3) },
                { FOURTH_ACTION, new CompositeEnemyAction(owner, new List<EnemyAction>()
                {
                    new ApplyPlayerStatusEffect(owner, burningEntity, 2, 2),
                    new ApplyPlayerStatusEffect(owner, deadlyPoisionEntity, 1, 2),
                    new ApplyPlayerStatusEffect(owner, bleedingEntity, 1, 2),
                })},
                { FIFTH_ACTION, new Payback(owner) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ SECOND_ACTION, THIRD_ACTION  },
                    condition : (context, remainActionCount) => owner.AsEntity.GetBattleStatusEffects(BattleStatusEffectType.DEBUFF).Count > 0
                ),
                new Pattern(
                    preset : new List<string>{ THIRD_ACTION, FIRST_ACTION  },
                    condition : (context, remainActionCount) => context.HandDeck.Count >= 5
                ),
                new Pattern(
                    preset : new List<string>{ FOURTH_ACTION  },
                    condition : (context, remainActionCount) => context.PlayerContainer.Player.GetBattleStatusEffects(BattleStatusEffectType.DEBUFF).Count <= 0
                ),
                new Pattern(
                    preset : new List<string>{ FIRST_ACTION, SECOND_ACTION, FIFTH_ACTION, FOURTH_ACTION  },
                    condition : (context, remainActionCount) => context.BattleDeckHistory.GetExecuteCardEffectCount(BattleScope.TURN) >= 10
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new UnknownEntity(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}