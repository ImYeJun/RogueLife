using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class GhostingTeammate : NormalBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "first";
        private const string SECOND_ACTION = "second";

        [SerializeField] private BattleStatusEffectData strengthenMuscleData;
        [SerializeField] private BattleStatusEffectData thatsFoulData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public GhostingTeammate() {}
        private GhostingTeammate(GhostingTeammate template, IEnemyBehaviourOwner owner) : base(owner)
        {
            strengthenMuscleData = template.strengthenMuscleData;
            thatsFoulData = template.thatsFoulData;

            availableActions = new Dictionary<string, Actions.EnemyAction>
            {
                { FIRST_ACTION, new ApplySelfStatusEffect(owner, strengthenMuscleData, 1, 2) },
                { SECOND_ACTION, new ApplySelfStatusEffect(owner, thatsFoulData, 1, 2) }
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
            return new GhostingTeammate(this, newOwner);
        }
    }
}