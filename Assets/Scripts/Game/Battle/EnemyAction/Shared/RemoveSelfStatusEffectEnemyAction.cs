using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle.Enemies.Actions.Shared
{
    public class RemoveItselfStatusEffect : EnemyAction
    {
        private readonly BattleStatusEffectType type;
        private int amount;

        public RemoveItselfStatusEffect(string id, IEnemyBehaviourOwner owner, BattleStatusEffectType type, int amount = 1) : base(id, owner)
        {
            this.amount = amount;
            this.type = type;
        }

        public override void Execute(BattleContext context)
        {
            var ownerAsEntity = owner.AsEntity;
            var ownerStatusEffects = ownerAsEntity.GetBattleStatusEffects(type);

            if (ownerStatusEffects.Count <= 0) { return; }

            var random = context.Random;
            var selectedStatusEffects = ownerStatusEffects.OrderBy(sel => random.Next()).Take(amount);

            foreach (var effect in selectedStatusEffects)
            {
                var removeEffectAction = new RemoveEntityStatusEffect(ownerAsEntity, effect);

                context.ActionScheduler.Enqueue(removeEffectAction);
            }
        }
    }
}