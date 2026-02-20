using System;
using System.ComponentModel;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class OmaeWaMoSindeIru : BattleStatusEffectBehaviour
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OmaeWaMoSindeIru() {}
        private OmaeWaMoSindeIru(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state, BattleStatusEffectData naniData) 
        : base(context, owner, state)
        {
            this.naniData = naniData;
        }

        [SerializeField] private BattleStatusEffectData naniData;

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribeActionModifier<RequestHurtEntityBattleAction>(YouAreAlreadyDead, PipelinePhaseStep.LATE);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribeActionModifier<RequestHurtEntityBattleAction>(YouAreAlreadyDead);
        }

        public void YouAreAlreadyDead(RequestHurtEntityBattleAction requestHurtEntityBattleAction, BattleContext context)
        {
            if (requestHurtEntityBattleAction.Source.Caster is not BattleEntity caster) { return; }
            if (caster != owner) { return; }
            
            var existingDamage = requestHurtEntityBattleAction.Damage;
            var target = requestHurtEntityBattleAction.Target;

            requestHurtEntityBattleAction.Nullify();

            var stack = (int)(existingDamage * 0.5f * state.StackCount);
            var nani = new BattleStatusEffect(naniData, stack);
            context.ActionScheduler.Enqueue(new ApplyEntityStatusEffectBattleAction(target, nani));
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new OmaeWaMoSindeIru(context, owner, state, naniData);
        }
    }
}