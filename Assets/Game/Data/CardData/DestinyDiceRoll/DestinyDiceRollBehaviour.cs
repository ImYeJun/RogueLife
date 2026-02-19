using System;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class DestinyDiceRoll : CardBattleBehaviour<SingleEnemyCardTarget, SingleEnemyCardTarget>
    {
        public override CardBattleBehaviour Clone()
        {
            return new DestinyDiceRoll();
        }

        public override bool IsAbleToUse(BattleContext context, CardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
            throw new NotImplementedException();
        }

        protected override void OnExecute(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            throw new NotImplementedException();
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            throw new NotImplementedException();
        }
    }
}