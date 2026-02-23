using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class RipoffSalesperson : NormalBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "first";
        private const string SECOND_ACTION = "second";

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RipoffSalesperson() {}
        private RipoffSalesperson(IEnemyBehaviourOwner owner) : base(owner)
        {
            availableActions = new Dictionary<string, Actions.EnemyAction>
            {
                { FIRST_ACTION, new DumpPlayerHandCard(owner)},
                { SECOND_ACTION, new HurtPlayer(owner, 20) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ FIRST_ACTION, FIRST_ACTION, SECOND_ACTION },
                    condition : (random, remainActionCount) => remainActionCount >= 3
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new RipoffSalesperson(newOwner);
        }
    }
}