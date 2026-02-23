using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class DrunkenMan : NormalBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "first";
        private const string SECOND_ACTION = "second";

        [SerializeField] private BattleStatusEffectData drunkenData;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DrunkenMan() {}
        private DrunkenMan(DrunkenMan template, IEnemyBehaviourOwner owner) : base(owner)
        {
            drunkenData = template.drunkenData;

            availableActions = new Dictionary<string, Actions.EnemyAction>
            {
                { FIRST_ACTION, new HurtPlayer(owner, 20) },
                { SECOND_ACTION, new ApplySelfStatusEffect(owner, drunkenData, 2, 2) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ SECOND_ACTION, SECOND_ACTION, FIRST_ACTION },
                    condition : (context, remainActionCount) => remainActionCount >= 3
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new DrunkenMan(this, newOwner);
        }
    }
}