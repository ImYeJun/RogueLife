using System;
using Battle.HurtSources;

namespace Belongingses.Behaviour
{
    [Serializable]
    public class BattleHeavyProteinBar : BattleBelongingsBehaviour
    {
        public override BattleBelongingsBehaviour Clone()
        {
            return new BattleHeavyProteinBar();
        }

        protected override void OnApplied()
        {
            context.EventBus.Subscribe<EntityHurtBattleEvent>(OnEntityHurt);
        }

        protected override void OnRemoved()
        {
            context.EventBus.Unsubscribe<EntityHurtBattleEvent>(OnEntityHurt);
        }

        public void OnEntityHurt(EntityHurtBattleEvent payload)
        {
            if (payload.Source is not CardSource cardSource) { return; }
            if (cardSource.SourceCard.CurrentAttribute != CardAttribute.PHYSICAL) { return; }

            var hurtAction = new RequestHurtEntityBattleAction(new NoneEntitySource(), 10, payload.Victim);
            context.ActionScheduler.Enqueue(hurtAction);
        }
    }
}