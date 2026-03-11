using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class Pickpocket : BossBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "first";
        private const string SECOND_ACTION = "second";
        private const string THIRD_ACTION = "third";
        private const string FOURTH_ACTION = "fourth";
        private const string FIFTH_ACTION = "fifth";

        [SerializeField] private BattleStatusEffectEntity dontTouchEntity;
        [SerializeField] private BattleStatusEffectEntity quickEscapeEntity;
        [SerializeField] private EnemyEntity obstacleForFleeingEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Pickpocket() {}
        private Pickpocket(Pickpocket template, IEnemyBehaviourOwner owner) : base(owner)
        {
            dontTouchEntity = template.dontTouchEntity;
            quickEscapeEntity = template.quickEscapeEntity;
            obstacleForFleeingEntity = template.obstacleForFleeingEntity;

            availableActions = new Dictionary<string, EnemyAction>
            {
                { FIRST_ACTION, new ApplySelfStatusEffect(owner, dontTouchEntity, 1, 2) },
                { SECOND_ACTION, new SpawnEnemy(owner, obstacleForFleeingEntity, 3) },
                { THIRD_ACTION, new ApplySelfStatusEffect(owner, quickEscapeEntity, 1, isLastAction : false, isOncePerTurn : true) },
                { FOURTH_ACTION, new HealSelf(owner, 20)},
                { FIFTH_ACTION, new DecreasePhaseCount(owner, 2) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ THIRD_ACTION  },
                    condition : (context, remainActionCount) => !owner.AsEntity.HasStatusEffect(quickEscapeEntity.Data)
                ),
                new Pattern(
                    preset : new List<string>{ SECOND_ACTION, FIRST_ACTION, FOURTH_ACTION  },
                    condition : (context, remainActionCount) => context.EnemySystem.GetEnemyCountByData(obstacleForFleeingEntity.Data) <= 2
                ),
                new Pattern(
                    preset : new List<string>{ FIFTH_ACTION, FOURTH_ACTION, FIRST_ACTION  },
                    condition : (context, remainActionCount) => context.EnemySystem.GetEnemyCountByData(obstacleForFleeingEntity.Data) >= 3
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new Pickpocket(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }
        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}