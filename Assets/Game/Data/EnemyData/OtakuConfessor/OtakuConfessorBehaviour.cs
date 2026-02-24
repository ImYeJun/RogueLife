using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class OtakuConfessor : NormalBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "first";
        private const string SECOND_ACTION = "second";

        [SerializeField] private BattleStatusEffectData strengthenMuscleData;
        [SerializeField] private BattleStatusEffectData toughenData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OtakuConfessor() {}
        private OtakuConfessor(OtakuConfessor template, IEnemyBehaviourOwner owner) : base(owner)
        {
            strengthenMuscleData = template.strengthenMuscleData;
            toughenData = template.toughenData;

            availableActions = new Dictionary<string, EnemyAction>
            {
                { FIRST_ACTION, new CompositeEnemyAction(owner, 
                    new List<EnemyAction>(){ 
                        new DirectlyDecreaseMentality(owner, 5),
                        new ApplyPlayerStatusEffect(owner, strengthenMuscleData, 1, 2),
                }) },
                { SECOND_ACTION, new ApplySelfStatusEffect(owner, toughenData, 2, 2) }
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
            return new OtakuConfessor(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}