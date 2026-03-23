using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using View.ScheduleView.Deck;
using UnityEngine.Pool;
using System.Collections.Generic;
using System.Linq;
using System;

namespace View.BattleView
{
    public class DeckInventoryView : InteractableViewBehaviour<IBattleViewEvent, IBattleViewCommander>
    {
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private GameObject cardSlotPrefab;
        [SerializeField] private Transform handDeckInventoty;
        [SerializeField] private Transform graveDeckInventoty;
        [SerializeField] private CardInspectorView cardInspectorView;

        private IReadOnlyBattleDeck drawDeck;
        private IReadOnlyBattleDeck graveDeck;

        private IObjectPool<BattleCardSlotView> drawDeckCards;
        private IObjectPool<BattleCardSlotView> graveDeckCards;
        
        private List<BattleCardSlotView> activeDrawDeckCardSlots = new List<BattleCardSlotView>();
        private List<BattleCardSlotView> activeGraveDeckCardSlots = new List<BattleCardSlotView>();

        private BattleCardSlotView focusedSlot;
        private Card focusedCard;

        public override void OnInitialized()
        {
            SetActive(false);
            cardInspectorView.GetStatusEffectData = commander.GetStatusEffectData;

            drawDeckCards = new ObjectPool<BattleCardSlotView>(
                createFunc: CreateDrawCardSlot,
                actionOnGet: GetCardSlot,
                actionOnRelease: ReturnCardSlot,
                actionOnDestroy: DestroyCardSlot,
                defaultCapacity: 30,
                maxSize: 10000
            );

            graveDeckCards = new ObjectPool<BattleCardSlotView>(
                createFunc: CreateGraveCardSlot,
                actionOnGet: GetCardSlot,
                actionOnRelease: ReturnCardSlot,
                actionOnDestroy: DestroyCardSlot,
                defaultCapacity: 30,
                maxSize: 10000
            );

            eventBus.Subscribe<InitialDeckSettled>(OnInitalDeckSettled);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialDeckSettled>(OnInitalDeckSettled);
        }

        public void SetActive(bool value)
        {
            if (value)
            {
                DrawInventory();
            }
            uiRoot.gameObject.SetActive(value);
        }

        private void OnInitalDeckSettled(InitialDeckSettled payload)
        {
            drawDeck = payload.DrawDeck;
            graveDeck = payload.GraveDeck;
        }

        private void DrawInventory()
        {
            ClearActiveSlots();
            focusedSlot = null;

            if (drawDeck != null)
            {
                foreach (var card in drawDeck.GetCards())
                {
                    var slot = drawDeckCards.Get();
                    
                    slot.Activate(card, OnCardSlotClicked); 
                    slot.transform.SetAsLastSibling();
                    activeDrawDeckCardSlots.Add(slot);
                }
            }

            if (graveDeck != null)
            {
                foreach (var card in graveDeck.GetCards())
                {
                    var slot = graveDeckCards.Get();
                    
                    slot.Activate(card, OnCardSlotClicked);
                    slot.transform.SetAsLastSibling();
                    activeGraveDeckCardSlots.Add(slot);
                }
            }
        }

        private void ClearActiveSlots()
        {
            foreach (var slot in activeDrawDeckCardSlots)
            {
                drawDeckCards.Release(slot);
            }
            activeDrawDeckCardSlots.Clear();

            foreach (var slot in activeGraveDeckCardSlots)
            {
                graveDeckCards.Release(slot);
            }
            activeGraveDeckCardSlots.Clear();
        }

        #region Object Pool Helper Methods
        private BattleCardSlotView CreateDrawCardSlot()
        {
            var cardSlotView = Instantiate(cardSlotPrefab, handDeckInventoty);
            cardSlotView.SetActive(false);
            return cardSlotView.GetComponent<BattleCardSlotView>();
        }
        private BattleCardSlotView CreateGraveCardSlot()
        {
            var cardSlotView = Instantiate(cardSlotPrefab, graveDeckInventoty);
            cardSlotView.SetActive(false);
            return cardSlotView.GetComponent<BattleCardSlotView>();
        }
        private void GetCardSlot(BattleCardSlotView view)
        {
            view.gameObject.SetActive(true);
        }
        private void ReturnCardSlot(BattleCardSlotView view)
        {
            view.OnUnfocus(); 
            view.gameObject.SetActive(false);
        }
        private void DestroyCardSlot(BattleCardSlotView view)
        {
            Destroy(view.gameObject);
        }

        #endregion

        private void OnCardSlotClicked(BattleCardSlotView slot)
        {
            focusedSlot?.OnUnfocus();
            
            focusedSlot = slot;
            focusedCard = slot.CurrentCard; 
            
            focusedSlot.OnFocused();

            cardInspectorView.VisualizeSelectedSlot(slot.CurrentCard);
        }
    }
}