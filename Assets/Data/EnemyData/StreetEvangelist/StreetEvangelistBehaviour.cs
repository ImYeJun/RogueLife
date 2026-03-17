using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class StreetEvangelist : NormalBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "Enemy_StreetEvangelist_Behavior_0";
        private const string SECOND_ACTION = "Enemy_StreetEvangelist_Behavior_1";

        [SerializeField] private BattleStatusEffectEntity heavyBodyEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public StreetEvangelist() {}
        private StreetEvangelist(StreetEvangelist template, IEnemyBehaviourOwner owner) : base(owner)
        {
            heavyBodyEntity = template.heavyBodyEntity;

            availableActions = new Dictionary<string, Actions.EnemyAction>
            {
                { FIRST_ACTION, new HurtPlayer(FIRST_ACTION, owner, 20) },
                { SECOND_ACTION, new ApplyPlayerStatusEffect(SECOND_ACTION, owner, heavyBodyEntity, 1, 2) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ SECOND_ACTION, SECOND_ACTION, FIRST_ACTION  },
                    condition : (context, remainActionCount) => remainActionCount >= 3
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new StreetEvangelist(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}