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
        private const string FIRST_ACTION = "Enemy_GhostingTeammate_Behavior_0";
        private const string SECOND_ACTION = "Enemy_GhostingTeammate_Behavior_1";

        [SerializeField] private BattleStatusEffectEntity strengthenMuscleEntity;
        [SerializeField] private BattleStatusEffectEntity thatsFoulEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public GhostingTeammate() {}
        private GhostingTeammate(GhostingTeammate template, IEnemyBehaviourOwner owner) : base(owner)
        {
            strengthenMuscleEntity = template.strengthenMuscleEntity;
            thatsFoulEntity = template.thatsFoulEntity;

            availableActions = new Dictionary<string, Actions.EnemyAction>
            {
                { FIRST_ACTION, new ApplySelfStatusEffect(FIRST_ACTION, owner, strengthenMuscleEntity, 1, 2) },
                { SECOND_ACTION, new ApplySelfStatusEffect(SECOND_ACTION, owner, thatsFoulEntity, 1, 2) }
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

        public override void OnOwnerDied(BattleContext context)
        {
        }
        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}