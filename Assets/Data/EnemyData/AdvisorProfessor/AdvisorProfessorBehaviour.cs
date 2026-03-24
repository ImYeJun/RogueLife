using System;
using System.Collections.Generic;
using System.ComponentModel;
using Battle.Enemies.Actions;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class AdvisorProfessor : BossBattleEnemyBehaviour
    {
        private const string FIRST_ACTION = "Enemy_AdvisorProfessor_Behavior_0";
        private const string SECOND_ACTION = "Enemy_AdvisorProfessor_Behavior_1";
        private const string THIRD_ACTION = "Enemy_AdvisorProfessor_Behavior_2";
        private const string FOURTH_ACTION = "Enemy_AdvisorProfessor_Behavior_3";
        private const string FIFTH_ACTION = "Enemy_AdvisorProfessor_Behavior_4";

        [SerializeField] private BattleStatusEffectEntity defensiveStanceEntity;
        [SerializeField] private BattleStatusEffectEntity iWillKillYouEntity;
        [SerializeField] private BattleStatusEffectEntity heavyBodyEntity;
        [SerializeField] private BattleStatusEffectEntity ohMyEntity;
        [SerializeField] private EnemyEntity labSlaveEntity;

        private class LabReorganization : EnemyAction
        {
            private EnemyEntity labSlaveEntity;

            public LabReorganization(string id, IEnemyBehaviourOwner owner, EnemyEntity labSlaveEntity) : base(id, owner, BattleEnemyActionType.Effect)
            {
                this.labSlaveEntity = labSlaveEntity;
            }

            public override void Execute(BattleContext context)
            {
                var labSlaves = context.EnemySystem.GetBattleEnemies(labSlaveEntity.Data);

                foreach (var graduate in labSlaves)
                {
                    var killAction = new KillEntityBattleAction(graduate);

                    context.ActionScheduler.Enqueue(killAction);
                }

                for (int i = 0; i < 4; i++)
                {
                    var spawnedGraduate = new BattleEnemy(context, labSlaveEntity);
                    var spawnAction = new SpawnEnemyBattleAction(spawnedGraduate);

                    context.ActionScheduler.Enqueue(spawnAction);
                }
            }
        }

        private class ForcedLabor : EnemyAction
        {
            private EnemyEntity labSlaveEntity;

            public ForcedLabor(string id, IEnemyBehaviourOwner owner, EnemyEntity labSlaveEntity, bool isLastAction = false, bool isOncePerTurn = false) : base(id, owner, BattleEnemyActionType.Effect, isLastAction, isOncePerTurn)
            {
                this.labSlaveEntity = labSlaveEntity;
            }

            public override void Execute(BattleContext context)
            {
                var labSlaves = context.EnemySystem.GetBattleEnemies(labSlaveEntity.Data);

                foreach (var graduate in labSlaves)
                {
                    var orderAttackAction = new RequestHurtEntityBattleAction(graduate.AsHurtSource, 10, context.PlayerContainer.Player);
                    context.ActionScheduler.Enqueue(orderAttackAction);
                }
            }
        }

        public class TakeCredit : EnemyAction
        {
            private EnemyEntity labSlaveEntity;

            public TakeCredit(string id, IEnemyBehaviourOwner owner, EnemyEntity labSlaveEntity, bool isLastAction = false, bool isOncePerTurn = false) : base(id, owner, BattleEnemyActionType.Effect, isLastAction, isOncePerTurn)
            {
                this.labSlaveEntity = labSlaveEntity;
            }
            public override void Execute(BattleContext context)
            {
                var labSlaves = context.EnemySystem.GetBattleEnemies(labSlaveEntity.Data);

                foreach (var graduate in labSlaves)
                {
                    var killAction = new KillEntityBattleAction(graduate);

                    int healAmount = graduate.CurrentHealth / 2;
                    var healAction = new HealEntityBattleAction(owner.AsEntity, healAmount);

                    context.ActionScheduler.Enqueue(killAction);
                    context.ActionScheduler.Enqueue(healAction);
                }
            }
        }

        private class PerformanceReview : EnemyAction
        {
            private EnemyEntity labSlaveEntity;
            private BattleStatusEffectEntity defensiveStanceData;
            private BattleStatusEffectEntity iWillKillYouData;
            private BattleStatusEffectEntity heavyBodyData;
            private BattleStatusEffectEntity ohMyData;

            // 💡 [수정된 부분] 첫 번째 인수로 id를 받고 base로 넘김
            public PerformanceReview(string id, IEnemyBehaviourOwner owner, EnemyEntity labSlaveEntity, BattleStatusEffectEntity defensiveStanceEntity, BattleStatusEffectEntity iWillKillYouEntity, BattleStatusEffectEntity heavyBodyEntity, BattleStatusEffectEntity ohMyEntity) : base(id, owner, BattleEnemyActionType.Effect)
            {
                this.labSlaveEntity = labSlaveEntity;
                this.defensiveStanceData = defensiveStanceEntity;
                this.iWillKillYouData = iWillKillYouEntity;
                this.heavyBodyData = heavyBodyEntity;
                this.ohMyData = ohMyEntity;
            }

            public override void Execute(BattleContext context)
            {
                var labSlaves = context.EnemySystem.GetBattleEnemies(labSlaveEntity.Data);

                BattleStatusEffect determinedStatusEffect;
                BattleEntity determinedTarget;
                switch (labSlaves.Count)
                {
                    case 0:
                        return;
                    case 1:
                        determinedStatusEffect = new BattleStatusEffect(defensiveStanceData, 2, 4);
                        determinedTarget = owner.AsEntity;
                        break;
                    case 2:
                        determinedStatusEffect = new BattleStatusEffect(iWillKillYouData, 4, 3);
                        determinedTarget = owner.AsEntity;
                        break;
                    case 3:
                        determinedStatusEffect = new BattleStatusEffect(heavyBodyData, 2, 2);
                        determinedTarget = context.PlayerContainer.Player;
                        break;
                    default:
                        determinedStatusEffect = new BattleStatusEffect(ohMyData, 1, 2);
                        determinedTarget = context.PlayerContainer.Player;
                        break;
                }

                var applyStatusEffectAction = new ApplyEntityStatusEffectBattleAction(determinedTarget, determinedStatusEffect);
                context.ActionScheduler.Enqueue(applyStatusEffectAction);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AdvisorProfessor() {}
        private AdvisorProfessor(AdvisorProfessor template, IEnemyBehaviourOwner owner) : base(owner)
        {
            defensiveStanceEntity = template.defensiveStanceEntity;
            iWillKillYouEntity = template.iWillKillYouEntity;
            heavyBodyEntity = template.heavyBodyEntity;
            ohMyEntity = template.ohMyEntity;
            labSlaveEntity = template.labSlaveEntity;

            availableActions = new Dictionary<string, EnemyAction>
            {
                { FIRST_ACTION, new HurtPlayer(FIRST_ACTION, owner, 30) },
                { SECOND_ACTION, new LabReorganization(SECOND_ACTION, owner, labSlaveEntity) },
                { THIRD_ACTION, new ForcedLabor(THIRD_ACTION, owner, labSlaveEntity) },
                { FOURTH_ACTION, new TakeCredit(FOURTH_ACTION, owner, labSlaveEntity)},
                { FIFTH_ACTION, new PerformanceReview(FIFTH_ACTION, owner, labSlaveEntity, defensiveStanceEntity, iWillKillYouEntity, heavyBodyEntity, ohMyEntity) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ FOURTH_ACTION, SECOND_ACTION  },
                    condition : (context, remainActionCount) => owner.AsEntity.CurrentHealth <= 100
                ),
                new Pattern(
                    preset : new List<string>{ FIRST_ACTION, THIRD_ACTION, FIFTH_ACTION },
                    condition : (context, remainActionCount) => context.EnemySystem.GetEnemyCountByData(labSlaveEntity.Data) >= 3
                ),
                new Pattern(
                    preset : new List<string>{ SECOND_ACTION, FIFTH_ACTION, THIRD_ACTION  },
                    condition : (context, remainActionCount) => context.EnemySystem.GetEnemyCountByData(labSlaveEntity.Data) <= 1
                )
            };
        }
        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new AdvisorProfessor(this, newOwner);
        }

        public override void OnOwnerDied(BattleContext context)
        {
            
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
        }
    }
}