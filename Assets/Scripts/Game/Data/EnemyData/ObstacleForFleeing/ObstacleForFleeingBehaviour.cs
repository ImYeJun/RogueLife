using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Battle.Enemies.Actions.Shared;
using UnityEngine;

namespace Battle.Enemies.Behaviours
{
    [Serializable]
    public class ObstacleForFleeing : NormalBattleEnemyBehaviour
    {
        [SerializeField] EnemyData pickPockectData;
        private BattleContext context;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ObstacleForFleeing() {}
        private ObstacleForFleeing(ObstacleForFleeing template, IEnemyBehaviourOwner owner) : base(owner)
        {
            pickPockectData = template.pickPockectData;
        }

        public override void OnOwnerSpawned(BattleContext context)
        {
            this.context = context;

            context.ActionObserverHub.SubscribePreObserver<UseCardBattleAction>(PreUseCard);
            context.EventBus.Subscribe<BattleEndBattleEvent>(OnBattleEnd);
        }
        public override void OnOwnerDied(BattleContext context)
        {
            CleanItself();
        }

        public void PreUseCard(UseCardBattleAction useCard, BattleContext context)
        {
            if (useCard.Target is SingleEnemyCardTarget enemyCardTarget)
            {
                if (enemyCardTarget.Enemy.Data != pickPockectData) { return; }
            }
            else if (useCard.Target is AllEnemyCardTarget allEnemyCardTarget)
            {
                if (!allEnemyCardTarget.Enemies.Any(enemy => enemy.Data == pickPockectData)) { return; }
            }
            else
            {
                return;
            }

            var hurtItself = new RequestHurtEntityBattleAction(owner.AsHurtSource, 1, owner.AsEntity);
            var hurtPlayer = new RequestHurtEntityBattleAction(owner.AsHurtSource, 20, context.PlayerContainer.Player);

            context.ActionScheduler.EnqueueFront(hurtPlayer);
            context.ActionScheduler.EnqueueFront(hurtItself);
        }

        public void OnBattleEnd(BattleEndBattleEvent payload)
        {
            CleanItself();
        }

        private void CleanItself()
        {
            context.ActionObserverHub.UnsubscribePreObserver<UseCardBattleAction>(PreUseCard);
            context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
        }

        public override BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner)
        {
            return new ObstacleForFleeing(this, newOwner);
        }

        
    }
}