using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class ExhaustedGraduateStudent : NormalBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "first";
        private const string SECOND_ACTION = "second";

        [SerializeField] private BattleStatusEffectData thatsFoulData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ExhaustedGraduateStudent() {}
        private ExhaustedGraduateStudent(ExhaustedGraduateStudent template, IEnemyBehaviourOwner owner) : base(owner)
        {
            thatsFoulData = template.thatsFoulData;

            availableActions = new Dictionary<string, Actions.EnemyAction>
            {
                { FIRST_ACTION, new HurtPlayer(owner, 20) },
                { SECOND_ACTION, new ApplySelfStatusEffect(owner, thatsFoulData, 1, 2) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ FIRST_ACTION, SECOND_ACTION  },
                    condition : (context, remainActionCount) => remainActionCount >= 3
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new ExhaustedGraduateStudent(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}