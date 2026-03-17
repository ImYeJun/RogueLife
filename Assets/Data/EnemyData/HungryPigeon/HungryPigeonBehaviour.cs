using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class HungryPigeon : BossBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "Enemy_HungryPigeon_Behavior_0";
        private const string SECOND_ACTION = "Enemy_HungryPigeon_Behavior_1";
        private const string THIRD_ACTION = "Enemy_HungryPigeon_Behavior_2";
        private const string FOURTH_ACTION = "Enemy_HungryPigeon_Behavior_3";
        private const string FIFTH_ACTION = "Enemy_HungryPigeon_Behavior_4";

        [SerializeField] private BattleStatusEffectEntity multiplyingComaradesEntity;
        [SerializeField] private BattleStatusEffectEntity toughenEntity;

        private class CallComarades : EnemyAction
        {
            public CallComarades(IEnemyBehaviourOwner owner, bool isLastAction = false) : base(owner, isLastAction)
            {
            }

            public override void Execute(BattleContext context)
            {
                if (owner is not BattleEnemy enemy) { return; }
                if (context.EnemySystem.GetEnemyCountByData(enemy.Data) <= 5)
                {
                    enemy.Clone(0.5f);
                } 
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HungryPigeon() {}
        private HungryPigeon(HungryPigeon template, IEnemyBehaviourOwner owner) : base(owner)
        {
            multiplyingComaradesEntity = template.multiplyingComaradesEntity;
            toughenEntity = template.toughenEntity;

            availableActions = new Dictionary<string, EnemyAction>
            {
                { FIRST_ACTION, new HurtPlayer(owner, 15) },
                { SECOND_ACTION, new ApplySelfStatusEffect(owner, multiplyingComaradesEntity, 1) },
                { THIRD_ACTION, new ApplySelfStatusEffect(owner, toughenEntity, 3, 2) },
                { FOURTH_ACTION, new HealSelf(owner, 20)},
                { FIFTH_ACTION, new CallComarades(owner) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ SECOND_ACTION, THIRD_ACTION  },
                    condition : (context, remainActionCount) => !owner.AsEntity.HasStatusEffect(multiplyingComaradesEntity.Data)
                ),
                new Pattern(
                    preset : new List<string>{ FIRST_ACTION, THIRD_ACTION, FIRST_ACTION  },
                    condition : (context, remainActionCount) => owner.AsEntity.CurrentHealth >= 50
                ),
                new Pattern(
                    preset : new List<string>{ SECOND_ACTION, THIRD_ACTION, FIFTH_ACTION  },
                    condition : (context, remainActionCount) => owner.AsEntity.CurrentHealth < 50
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new HungryPigeon(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}