using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class BlockingTrafficLight  : EliteBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "Enemy_BlockingTrafficLight_Behavior_0";
        private const string SECOND_ACTION = "Enemy_BlockingTrafficLight_Behavior_1";
        private const string THIRD_ACTION = "Enemy_BlockingTrafficLight_Behavior_2";

        [SerializeField] private BattleStatusEffectEntity nanoMachineEntity;
        [SerializeField] private BattleStatusEffectEntity heavyBodyEntity;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BlockingTrafficLight() {}
        private BlockingTrafficLight(BlockingTrafficLight template, IEnemyBehaviourOwner owner) : base(owner)
        {
            nanoMachineEntity = template.nanoMachineEntity;
            heavyBodyEntity = template.heavyBodyEntity;

            availableActions = new Dictionary<string, EnemyAction>
            {
                { FIRST_ACTION, new CompositeEnemyAction(owner, new List<EnemyAction>()
                {
                    new HealSelf(owner, 50),
                    new RemoveItselfStatusEffect(owner, BattleStatusEffectType.DEBUFF, 1)
                }) },
                { SECOND_ACTION, new ApplySelfStatusEffect(owner, nanoMachineEntity, 1, 4) },
                { THIRD_ACTION, new ApplyPlayerStatusEffect(owner, heavyBodyEntity, 2, 2) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ THIRD_ACTION, THIRD_ACTION, SECOND_ACTION, SECOND_ACTION },
                    condition : (context, remainActionCount) => remainActionCount >= 4
                ),
                new Pattern(
                    preset : new List<string> { SECOND_ACTION, FIRST_ACTION },
                    condition : (context, remainActionCount) => owner.AsEntity.CurrentHealth <= 80
                )
            };
        }
        
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new BlockingTrafficLight(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}