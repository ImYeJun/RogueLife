using System;
using System.Linq;

namespace Battle.Enemies.Actions.Shared
{
    public class ClearSelfStatusEffect : EnemyAction
    {
        private readonly BattleStatusEffectType type;

        public ClearSelfStatusEffect(string id, IEnemyBehaviourOwner owner, BattleStatusEffectType type = BattleStatusEffectType.ANY) : base(id, owner, BattleEnemyActionType.Effect)
        {
            this.type = type;
        }

        public override void Execute(BattleContext context)
        {
            var ownerAsEntity = owner.AsEntity;
            var ownerStatsuEffects = ownerAsEntity.GetBattleStatusEffects(type);

            foreach (var effect in ownerStatsuEffects)
            {
                var removeEffectAction = new RemoveEntityStatusEffect(ownerAsEntity, effect);

                context.ActionScheduler.Enqueue(removeEffectAction);
            }
        }
    }
}