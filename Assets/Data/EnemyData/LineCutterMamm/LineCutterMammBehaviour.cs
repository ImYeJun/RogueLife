using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class LineCutterMamm : NormalBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "Enemy_LineCutterMamm_Behavior_0";
        private const string SECOND_ACTION = "Enemy_LineCutterMamm_Behavior_1";

        [SerializeField] private BattleStatusEffectEntity counterAttackEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public LineCutterMamm() {}
        private LineCutterMamm(LineCutterMamm template, IEnemyBehaviourOwner owner) : base(owner)
        {
            counterAttackEntity = template.counterAttackEntity;

            availableActions = new Dictionary<string, Actions.EnemyAction>
            {
                { FIRST_ACTION, new ApplySelfStatusEffect(FIRST_ACTION, owner, counterAttackEntity, 2, 2) },
                { SECOND_ACTION, new HealSelf(SECOND_ACTION, owner, 10) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ SECOND_ACTION, FIRST_ACTION, FIRST_ACTION },
                    condition : (context, remainActionCount) => remainActionCount >= 3
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new LineCutterMamm(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}