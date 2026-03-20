using System;
using System.Collections.Generic;
using System.ComponentModel;
using Field.Deck.Observers;

namespace Battle.StatusEffects.Behaviour
{
    [Serializable]
    public class LightBody : DisposableBattleStatusEffectBehaviour
    {
        private Observer observer;

        private class Observer : IDeckObserver
        {
            private BattleContext context;
            private LightBody parent;
            
            private Dictionary<Card, CardCostModifier> modifiers;

            public Observer(BattleContext context, LightBody parent)
            {
                this.context = context;
                this.parent = parent;
                this.modifiers = new Dictionary<Card, CardCostModifier>();
            }

            public void OnStartObserving(List<Card> owningCards)
            {
                foreach (var card in owningCards)
                {
                    ApplyModifier(card);
                }

                context.ActionObserverHub.SubscribePreObserver<UseCardBattleAction>(OnUseCard);
                context.EventBus.Subscribe<BattleEndBattleEvent>(OnBattleEnd);
            }

            public void OnCardEquipped(Card card)
            {
                ApplyModifier(card);
            }

            public void OnCardRemoved(Card card)
            {
                RemoveModifier(card);
            }

            public void OnStopObserving(List<Card> owningCards)
            {
                Clean();
            }

            private void ApplyModifier(Card card)
            {
                if (!modifiers.ContainsKey(card))
                {
                    var mod = new CardCostModifier(-parent.state.StackCount);
                    // 💡 [수정됨] 직접 추가 대신 Action을 큐에 삽입합니다.
                    context.ActionScheduler.Enqueue(new AddCardCostModifierBattleAction(card, mod));
                    modifiers[card] = mod;
                }
            }

            private void RemoveModifier(Card card)
            {
                if (modifiers.TryGetValue(card, out var mod))
                {
                    // 💡 [수정됨] 직접 제거 대신 Action을 큐에 삽입합니다.
                    context.ActionScheduler.Enqueue(new RemoveCardCostModifierBattleAction(card, mod));
                    modifiers.Remove(card);
                }
            }

            public void UpdateModifiers()
            {
                var keys = new List<Card>(modifiers.Keys);
                foreach (var card in keys)
                {
                    // 💡 [수정됨]
                    context.ActionScheduler.Enqueue(new RemoveCardCostModifierBattleAction(card, modifiers[card]));
                    
                    var newMod = new CardCostModifier(-parent.state.StackCount);
                    context.ActionScheduler.Enqueue(new AddCardCostModifierBattleAction(card, newMod));
                    modifiers[card] = newMod;
                }
            }

            public void OnUseCard(UseCardBattleAction action, BattleContext context)
            {
                if (modifiers.ContainsKey(action.Card))
                {
                    parent.OnExecuted();     
                    parent.RequestExpire();  
                }
            }

            public void OnBattleEnd(BattleEndBattleEvent payload)
            {
                Clean();
            }

            private void Clean()
            {
                var keys = new List<Card>(modifiers.Keys);
                foreach (var card in keys)
                {
                    // 💡 [수정됨]
                    context.ActionScheduler.Enqueue(new RemoveCardCostModifierBattleAction(card, modifiers[card]));
                }
                modifiers.Clear();

                context.ActionObserverHub.UnsubscribePreObserver<UseCardBattleAction>(OnUseCard);
                context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
            }
        }

        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public LightBody() {}
        private LightBody(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state) 
        : base(context, owner, state) { }

        public override BattleStatusEffectBehaviour Clone(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
        {
            return new LightBody(context, owner, state);
        }

        public override void OnApplied()
        {
            observer = new Observer(context, this);
            
            context.DeckSystem.RegisterHandDeckObserver(observer);
        }

        public override void OnMerged()
        {
            observer?.UpdateModifiers();
        }

        public override void OnRemoved(bool isOwnerDied = false)
        {
            if (observer != null)
            {
                context.DeckSystem.UnregisterHandDeckObserver(observer);
                observer = null;
            }
        }
    }
}