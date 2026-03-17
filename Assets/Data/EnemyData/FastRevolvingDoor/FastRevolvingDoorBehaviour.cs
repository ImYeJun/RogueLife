using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions;
using Battle.Enemies.Actions.Shared;
using Battle.HurtSources;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class FastRevolvingDoor : EliteBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "Enemy_FastRevolvingDoor_Behavior_0";
        private const string SECOND_ACTION = "Enemy_FastRevolvingDoor_Behavior_1";
        private const string THIRD_ACTION = "Enemy_FastRevolvingDoor_Behavior_2";

        [SerializeField] private BattleStatusEffectEntity bleedingEntity;

        private class RecklessSpin : EnemyAction
        {
            private class Observer
            {
                private IEnemyBehaviourOwner owner;
                private BattleContext context;
                private BattleHurtSource hurtSource;

                public Observer(IEnemyBehaviourOwner owner, BattleContext context, BattleHurtSource hurtSource)
                {
                    this.owner = owner;
                    this.context = context;
                    this.hurtSource = hurtSource;
                }

                public void OnEntityHurt(EntityHurtBattleEvent payload)
                {
                    if (payload.Source != hurtSource) { return; }

                    var hurtPlayerAction = new RequestHurtEntityBattleAction(owner.AsHurtSource, payload.Amount, context.PlayerContainer.Player);
                    context.ActionScheduler.Enqueue(hurtPlayerAction);

                    CleanItself();
                }  

                public void OnBattleEnd(BattleEndBattleEvent payload)
                {
                    CleanItself();
                }

                private void CleanItself()
                {
                    context.EventBus.Unsubscribe<EntityHurtBattleEvent>(OnEntityHurt);
                    context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
                }
            }

            public RecklessSpin(IEnemyBehaviourOwner owner) : base(owner)
            {
            }

            public override void Execute(BattleContext context)
            {
                var ownerAsEntity = owner.AsEntity;

                int damage = 20 + ((ownerAsEntity.MaxHealth - ownerAsEntity.CurrentHealth) / 2);

                var hurtSource = owner.AsHurtSource;

                var observer = new Observer(owner, context, hurtSource);
                context.EventBus.Subscribe<EntityHurtBattleEvent>(observer.OnEntityHurt);
                context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);

                var hurtAction = new RequestHurtEntityBattleAction(hurtSource, damage, ownerAsEntity);
                context.ActionScheduler.Enqueue(hurtAction);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FastRevolvingDoor() {}
        private FastRevolvingDoor(FastRevolvingDoor template, IEnemyBehaviourOwner owner) : base(owner)
        {
            bleedingEntity = template.bleedingEntity;

            availableActions = new Dictionary<string, EnemyAction>
            {
                { FIRST_ACTION, new CompositeEnemyAction(owner, new List<EnemyAction>()
                {
                    new HurtPlayer(owner, 20),
                    new ApplyPlayerStatusEffect(owner, bleedingEntity, 2, 2)
                }) },
                { SECOND_ACTION, new RecklessSpin(owner) },
                { THIRD_ACTION, new HurtSelf(owner, 40) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ THIRD_ACTION, FIRST_ACTION, FIRST_ACTION, SECOND_ACTION },
                    condition : (context, remainActionCount) => remainActionCount >= 4
                ),
                new Pattern(
                    preset : new List<string> { THIRD_ACTION, THIRD_ACTION, SECOND_ACTION },
                    condition : (context, remainActionCount) => owner.AsEntity.CurrentHealth >= 100
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new FastRevolvingDoor(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}