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
        private const string FIRST_ACTION = "first";
        private const string SECOND_ACTION = "second";
        private const string THIRD_ACTION = "third";
        private const string FOURTH_ACTION = "fourth";
        private const string FIFTH_ACTION = "fifth";

        [SerializeField] private BattleStatusEffectData defensiveStanceData;
        [SerializeField] private BattleStatusEffectData iWillKillYouData;
        [SerializeField] private BattleStatusEffectData heavyBodyData;
        [SerializeField] private BattleStatusEffectData ohMyData;
        [SerializeField] private EnemyData labSlaveData;

        private class LabReorganization : EnemyAction
        {
            private EnemyData labSlaveData;

            public LabReorganization(IEnemyBehaviourOwner owner, EnemyData labSlaveData) : base(owner)
            {
                this.labSlaveData = labSlaveData;
            }

            public override void Execute(BattleContext context)
            {
                var labSlaves = context.EnemySystem.GetBattleEnemies(labSlaveData);

                foreach (var graduate in labSlaves)
                {
                    var killAction = new KillEntityBattleAction(graduate);

                    context.ActionScheduler.Enqueue(killAction);
                }

                for (int i = 0; i < 4; i++)
                {
                    var spawnedGraduate = new BattleEnemy(context, labSlaveData);
                    var spawnAction = new SpawnEnemyBattleAction(spawnedGraduate);

                    context.ActionScheduler.Enqueue(spawnAction);
                }
            }
        }

        private class ForcedLabor : EnemyAction
        {
            private EnemyData labSlaveData;

            public ForcedLabor(IEnemyBehaviourOwner owner, EnemyData labSlaveData, bool isLastAction = false, bool isOncePerTurn = false) : base(owner, isLastAction, isOncePerTurn)
            {
                this.labSlaveData = labSlaveData;
            }

            public override void Execute(BattleContext context)
            {
                var labSlaves = context.EnemySystem.GetBattleEnemies(labSlaveData);

                foreach (var graduate in labSlaves)
                {
                    var orderAttackAction = new RequestHurtEntityBattleAction(graduate.AsHurtSource, 10, context.PlayerContainer.Player);
                    context.ActionScheduler.Enqueue(orderAttackAction);
                }
            }
        }

        public class TakeCredit : EnemyAction
        {
            private EnemyData labSlaveData;

            public TakeCredit(IEnemyBehaviourOwner owner, EnemyData labSlaveData, bool isLastAction = false, bool isOncePerTurn = false) : base(owner, isLastAction, isOncePerTurn)
            {
                this.labSlaveData = labSlaveData;
            }
            public override void Execute(BattleContext context)
            {
                var labSlaves = context.EnemySystem.GetBattleEnemies(labSlaveData);

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
            private EnemyData labSlaveData;
            private BattleStatusEffectData defensiveStanceData;
            private BattleStatusEffectData iWillKillYouData;
            private BattleStatusEffectData heavyBodyData;
            private BattleStatusEffectData ohMyData;

            public PerformanceReview(IEnemyBehaviourOwner owner, EnemyData labSlaveData, BattleStatusEffectData defensiveStanceData, BattleStatusEffectData iWillKillYouData, BattleStatusEffectData heavyBodyData, BattleStatusEffectData ohMyData) : base(owner)
            {
                this.labSlaveData = labSlaveData;
                this.defensiveStanceData = defensiveStanceData;
                this.iWillKillYouData = iWillKillYouData;
                this.heavyBodyData = heavyBodyData;
                this.ohMyData = ohMyData;
            }

            public override void Execute(BattleContext context)
            {
                var labSlaves = context.EnemySystem.GetBattleEnemies(labSlaveData);

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
            defensiveStanceData = template.defensiveStanceData;
            iWillKillYouData = template.iWillKillYouData;
            heavyBodyData = template.heavyBodyData;
            ohMyData = template.ohMyData;
            labSlaveData = template.labSlaveData;

            availableActions = new Dictionary<string, EnemyAction>
            {
                { FIRST_ACTION, new HurtPlayer(owner, 30) },
                { SECOND_ACTION, new LabReorganization(owner, labSlaveData) },
                { THIRD_ACTION, new ForcedLabor(owner, labSlaveData) },
                { FOURTH_ACTION, new TakeCredit(owner, labSlaveData)},
                { FIFTH_ACTION, new PerformanceReview(owner, labSlaveData, defensiveStanceData, iWillKillYouData, heavyBodyData, ohMyData) }
            };

            availablePatterns = new List<Pattern>
            {
                new Pattern(
                    preset : new List<string>{ FOURTH_ACTION, SECOND_ACTION  },
                    condition : (context, remainActionCount) => owner.AsEntity.CurrentHealth <= 100
                ),
                new Pattern(
                    preset : new List<string>{ FIRST_ACTION, THIRD_ACTION, FIFTH_ACTION },
                    condition : (context, remainActionCount) => context.EnemySystem.GetEnemyCountByData(labSlaveData) >= 3
                ),
                new Pattern(
                    preset : new List<string>{ SECOND_ACTION, FIFTH_ACTION, THIRD_ACTION  },
                    condition : (context, remainActionCount) => context.EnemySystem.GetEnemyCountByData(labSlaveData) <= 1
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