using System;
using System.ComponentModel;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class WaterFist : BattleStatusEffectBehaviour, IBattleActionModifier
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WaterFist() {}
        private WaterFist(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier(this);
        }

        public override void ActivateEffect()
        {
            //TODO Delete this Method. It's useless!
        }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier(this);
        }


        public void ModifyAction(IBattleAction action, BattleContext context)
        {
            if (action is RequestHurtEntityBattleAction requestHurtEntityBattleAction)
            {
                if (requestHurtEntityBattleAction.Source is EntityBattleHurtSource entityBattleHurtSource)
                {
                    if (entityBattleHurtSource.SourceEntity != owner) return;

                    requestHurtEntityBattleAction.ReduceDamage(state.StackCount * 5);
                }
            }
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new WaterFist(context, owner, state);
        }
    }
}