using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Remoting.Contexts;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class LabSlave : BattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "Enemy_ExhaustedGraduateStudent_Behavior_0";
        private const string SECOND_ACTION = "Enemy_ExhaustedGraduateStudent_Behavior_1";

        [SerializeField] private BattleStatusEffectEntity thatsFoulEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public LabSlave() {}
        private LabSlave(LabSlave template, IEnemyBehaviourOwner owner) : base(owner)
        {
            thatsFoulEntity = template.thatsFoulEntity;

            availableActions = new Dictionary<string, Actions.EnemyAction>
            {
                { FIRST_ACTION, new HurtPlayer(owner, 20) },
                { SECOND_ACTION, new ApplySelfStatusEffect(owner, thatsFoulEntity, 1, 2) }
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new LabSlave(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }

        protected override int CalculateActionCount(System.Random random)
        {
            return 1;
        }
    }
}