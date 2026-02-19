using System;
using System.ComponentModel;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class DestinyDiceRoll : CardBattleBehaviour<SingleEnemyCardTarget, SingleEnemyCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DestinyDiceRoll() {}
        private DestinyDiceRoll(ICardBehaviourOwner owner) 
        : base(owner) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new DestinyDiceRoll(owner);
        }

        public override bool IsAbleToUse(BattleContext context, CardTarget target)
        {
            return true;
        }

        public override bool IsAbleToUseReflect(BattleContext context, CardTarget target)
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