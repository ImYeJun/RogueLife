using System;
using System.ComponentModel;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class DontTouch : DisposableBattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DontTouch() {}
        private DontTouch(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<BattleEntityAction>(NullifyIfTouched);
        }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<BattleEntityAction>(NullifyIfTouched);
        }

        public override void OnMerged() { }

        public void NullifyIfTouched(BattleEntityAction entityAction, BattleContext context )
        {
            if (entityAction.Actor == owner) return; 
            
            var intendedAction = entityAction.Action;
            if (intendedAction is IEntityTargetedBattleAction targetedAction)
            {
                if (targetedAction.Target == owner)
                {
                    OnExecuted();
                    entityAction.Nullify();
                    RequestExpire();
                }
            }
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new DontTouch(context, owner, state);
        }
    }
}