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
        private const string FIRST_ACTION = "first";
        private const string SECOND_ACTION = "second";

        [SerializeField] private BattleStatusEffectData counterAttack;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public LineCutterMamm() {}
        private LineCutterMamm(LineCutterMamm template, IEnemyBehaviourOwner owner) : base(owner)
        {
            counterAttack = template.counterAttack;

            availableActions = new Dictionary<string, Actions.EnemyAction>
            {
                { FIRST_ACTION, new ApplySelfStatusEffect(owner, counterAttack, 2, 2) },
                { SECOND_ACTION, new HealSelf(owner, 10) }
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
    }
}