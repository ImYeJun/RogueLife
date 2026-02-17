using System;
using System.ComponentModel;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Battle.StatusEffect.Behaviour
{
    [Serializable]
    public class OmaeWaMoSindeIru : BattleStatusEffectBehaviour, IBattleActionPreObserver
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public OmaeWaMoSindeIru() {}
        private OmaeWaMoSindeIru(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        [SerializeField] private BattleStatusEffectData naniData;

        public override void OnApplied()
        {
            context.ActionObserverHub.SubscribePreObserver(this);
        }

        public override void OnMerged() { }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            context.ActionObserverHub.UnsubscribePreObserver(this);
        }

        public void PreObserveAction(IBattleAction action, BattleContext context)
        {
            if (action is RequestHurtEntityBattleAction requestHurtEntityBattleAction)
            {
                var damage = requestHurtEntityBattleAction.Damage;
                //* 무효화
                var target = requestHurtEntityBattleAction.Target;

                var stack = (int)(damage * 0.5);
                var nani = new BattleStatusEffect(naniData, stack);
                context.ActionScheduler.Enqueue(new ApplyEntityStatusEffectBattleAction(target, nani));
            }
        }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new OmaeWaMoSindeIru(context, owner, state);
        }
    }
}