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
        private const string FIRST_ACTION = "Enemy_RipoffSalesperson_Behavior_0";
        private const string SECOND_ACTION = "Enemy_RipoffSalesperson_Behavior_1";

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RipoffSalesperson() {}
        private RipoffSalesperson(IEnemyBehaviourOwner owner) : base(owner)
        {
            availableActions = new Dictionary<string, Actions.EnemyAction>
            {
                { FIRST_ACTION, new DumpPlayerHandCard(FIRST_ACTION, owner)},
                { SECOND_ACTION, new HurtPlayer(SECOND_ACTION, owner, 20) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ FIRST_ACTION, FIRST_ACTION, SECOND_ACTION },
                    condition : (context, remainActionCount) => remainActionCount >= 3
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new RipoffSalesperson(newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}