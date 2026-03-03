using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.Deck
{
    public class DeckInventoryView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
    {
        private IReadOnlyDeck playerDeck;
        private CardSlotView focusedSlot;
        private Card focusedCard; 
        
        private DeckInventorySorter deckSorter = new DeckInventorySorter();
        [SerializeField] private SortingSettingView sortingSettingView;
        [SerializeField] private FilteringSettingView filteringSettingView;

        [SerializeField] private UnityEvent<Card> OnSlotClicked;

        [SerializeField] private GameObject mainCardSlotPrefab;
        [SerializeField] private GameObject sideCardSlotPrefab;
        [SerializeField] private Transform mainDeckInventory;
        [SerializeField] private Transform sideDeckInventory;

        [SerializeField] private TextMeshProUGUI mainDeckIndicator;
        [SerializeField] private TextMeshProUGUI sideDeckIndicator;

        private IObjectPool<CardSlotView> mainDeckPool;
        private IObjectPool<CardSlotView> sideDeckPool;
        private List<CardSlotView> activeMainDeckSlots = new List<CardSlotView>();
        private List<CardSlotView> activeSideDeckSlots = new List<CardSlotView>();

        public override void OnInitialized()
        {
            mainDeckPool = new ObjectPool<CardSlotView>(
                createFunc : CreateMainCardSlot,
                actionOnGet : GetDeckCardSlot,
                actionOnRelease : ReturnDeckCardSlot,
                actionOnDestroy : DestroyDeckCardSlot,
                defaultCapacity : Constant.MAX_MAIN_DECK_CARD_TYPE_COUNT * Constant.BASE_MAX_COPIES_PER_CARD,
                maxSize : 100
            );
            sideDeckPool = new ObjectPool<CardSlotView>(
                createFunc : CreateSideCardSlot,
                actionOnGet : GetDeckCardSlot,
                actionOnRelease : ReturnDeckCardSlot,
                actionOnDestroy : DestroyDeckCardSlot,
                defaultCapacity : Constant.MAX_SIDE_DECK_CARD_TYPE_COUNT * Constant.BASE_MAX_COPIES_PER_CARD,
                maxSize : 100
            );

            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus.Subscribe<DeckChanged>(OnDeckChanged);

            sortingSettingView.SetOnButtonPressed(ChangeSortingState);
            filteringSettingView.SetOnButtonPressed(ToggleAttributeFilteringState, ToggleTypeFilteringState, ToggleCostFilteringState);
        }

        public void OnDeckChanged(DeckChanged payload)
        {
            playerDeck = payload.Deck;
            DrawView();
        }

        private CardSlotView CreateMainCardSlot()
        {
            var cardSlotView = Instantiate(mainCardSlotPrefab, mainDeckInventory);
            return cardSlotView.GetComponent<CardSlotView>();
        }
        private CardSlotView CreateSideCardSlot()
        {
            var cardSlotView = Instantiate(sideCardSlotPrefab, sideDeckInventory);
            return cardSlotView.GetComponent<CardSlotView>();
        }
        private void GetDeckCardSlot(CardSlotView view)
        {
            view.gameObject.SetActive(true);
        }
        private void ReturnDeckCardSlot(CardSlotView view)
        {
            view.OnUnfocus(); 
            view.gameObject.SetActive(false);
        }
        private void DestroyDeckCardSlot(CardSlotView view)
        {
            Destroy(view.gameObject);
        }

        public override void OnDestroy()
        {
            eventBus.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
        }

        public void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            InitializeView();
            playerDeck = payload.Deck;
        }

        public void InitializeView()
        {
            deckSorter.Initialize();
            sortingSettingView.Initialize();
            filteringSettingView.Initialize();

            focusedSlot = null;
            focusedCard = null;

            ClearActiveSlots();
        }

        public void OnViewOpened()
        {
            gameObject.SetActive(true);
            DrawView();
        }

        private void DrawView()
        {
            ClearActiveSlots();
            focusedSlot = null;

            sortingSettingView.SetState(deckSorter.SortingState);
            filteringSettingView.SetState((deckSorter.FilteringAttributes, deckSorter.FilteringType, deckSorter.FilteringCost));

            DrawInventory(playerDeck.MainDeck, DeckType.MAIN_DECK);
            DrawInventory(playerDeck.SideDeck, DeckType.SIDE_DECK);
        }

        private void DrawInventory(IReadOnlyDictionary<CardData, List<Card>> deck, DeckType type)
        {
            IObjectPool<CardSlotView> pool = type switch
            {
                DeckType.MAIN_DECK => mainDeckPool,
                DeckType.SIDE_DECK => sideDeckPool,
                _ => throw new InvalidCastException($"[DeckInventoryView] {type} is not valid.")
            };
            
            List<Card> processedDeck = deckSorter.ProcessDeck(deck);

            foreach (var card in processedDeck)
            {
                var slot = pool.Get();
                slot.Activate(card, NotifySlotClicked, commander);
                slot.transform.SetAsLastSibling();

                if (focusedCard != null && card == focusedCard)
                {
                    focusedSlot = slot;
                    focusedSlot.OnFocused();
                }

                if (type == DeckType.MAIN_DECK) { activeMainDeckSlots.Add(slot); }
                else if (type == DeckType.SIDE_DECK) { activeSideDeckSlots.Add(slot); }
            }

            switch (type)
            {
                case DeckType.MAIN_DECK:
                    mainDeckIndicator.text = $"전투 덱 (전투 덱 카드 종류 : {deck.Count}/{Constant.MAX_MAIN_DECK_CARD_TYPE_COUNT})";
                    break;
                case DeckType.SIDE_DECK:
                    sideDeckIndicator.text = $"보조 덱 (보조 덱 카드 종류 : {deck.Count}/{Constant.MAX_SIDE_DECK_CARD_TYPE_COUNT})";
                    break;
                default:
                    throw new InvalidCastException($"[DeckInventoryView] {type} is not valid.");
            }
        }
        
        public void NotifySlotClicked(CardSlotView slotView)
        {
            focusedSlot?.OnUnfocus();
            
            focusedSlot = slotView;
            focusedCard = slotView.CurrentCard; 
            
            focusedSlot.OnFocused();

            OnSlotClicked.Invoke(slotView.CurrentCard);
        }

        private void ClearActiveSlots()
        {
            foreach (var slot in activeMainDeckSlots)
            {
                mainDeckPool.Release(slot);
            }
            foreach (var slot in activeSideDeckSlots)
            {
                sideDeckPool.Release(slot);
            }
            activeMainDeckSlots.Clear();
            activeSideDeckSlots.Clear();
        }

        public void ChangeSortingState(SortingType type) 
        { 
            deckSorter.ChangeSortingState(type);
            DrawView();
        }

        public void ToggleAttributeFilteringState(CardAttribute attribute) 
        { 
            deckSorter.ToggleAttributeFilter(attribute);
            DrawView();
        }
        public void ToggleTypeFilteringState(CardType type) 
        { 
            deckSorter.ToggleTypeFilter(type);
            DrawView();
        }
        public void ToggleCostFilteringState(int cost) 
        { 
            deckSorter.ToggleCostFilter(cost);
            DrawView();
        }
    }
}