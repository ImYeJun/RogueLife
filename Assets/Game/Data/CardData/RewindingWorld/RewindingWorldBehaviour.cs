using System;
using System.ComponentModel;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class RewindingWorld : CardBattleBehaviour<NoneCardTarget, NoneCardTarget>
    {
        private bool isCheckingCardUse = false;

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RewindingWorld() {}
        private RewindingWorld(ICardBehaviourOwner owner) 
        : base(owner) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new RewindingWorld(owner);
        }

        public override bool OnIsAbleToUse(BattleContext context, NoneCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, NoneCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
            if (isCheckingCardUse) { return; }


        }

        protected override void OnExecute(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            throw new NotImplementedException();
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, NoneCardTarget target)
        {
            throw new NotImplementedException();
        }
    }
}