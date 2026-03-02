using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class CartRidingKids  : EliteBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "first";
        private const string SECOND_ACTION = "second";
        private const string THIRD_ACTION = "third";

        [SerializeField] private BattleStatusEffectEntity strenghenMuscleEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CartRidingKids() {}
        private CartRidingKids(CartRidingKids template, IEnemyBehaviourOwner owner) : base(owner)
        {
            strenghenMuscleEntity = template.strenghenMuscleEntity;

            availableActions = new Dictionary<string, EnemyAction>
            {
                { FIRST_ACTION, new HurtPlayer(owner, 40, true) },
                { SECOND_ACTION, new ApplySelfStatusEffect(owner, strenghenMuscleEntity, 2, 2) },
                { THIRD_ACTION, new RemoveItselfStatusEffect(owner, BattleStatusEffectType.DEBUFF) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ THIRD_ACTION, SECOND_ACTION, SECOND_ACTION, FIRST_ACTION },
                    condition : (context, remainActionCount) => remainActionCount >= 4
                ),
                new Pattern(
                    preset : new List<string> { THIRD_ACTION, THIRD_ACTION },
                    condition : (context, remainActionCount) => owner.AsEntity.GetBattleStatusEffects(BattleStatusEffectType.DEBUFF).Count >= 2
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new CartRidingKids(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }
        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}