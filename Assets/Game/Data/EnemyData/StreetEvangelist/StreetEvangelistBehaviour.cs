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
        private const string FIRST_ACTION = "first";
        private const string SECOND_ACTION = "second";

        [SerializeField] private BattleStatusEffectData heavyBodyData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public StreetEvangelist() {}
        private StreetEvangelist(StreetEvangelist template, IEnemyBehaviourOwner owner) : base(owner)
        {
            heavyBodyData = template.heavyBodyData;

            availableActions = new Dictionary<string, Actions.EnemyAction>
            {
                { FIRST_ACTION, new HurtPlayer(owner, 20) },
                { SECOND_ACTION, new ApplyPlayerStatusEffect(owner, heavyBodyData, 1, 2) }
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
    }
}