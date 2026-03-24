using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityEngine.UI;
using View.Core;
using View.Global;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.Deck
{
    public class DeckInventoryView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
    {
        private IReadOnlyDeck playerDeck;
        private CardSlotView focusedSlot;
        private Card focusedCard; 
        
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private CardInspectorView cardInspectorView;

        private DeckInventorySorter deckSorter = new DeckInventorySorter();
        [SerializeField] private SortingSettingView sortingSettingView;
        [SerializeField] private FilteringSettingView filteringSettingView;

        [SerializeField] private GameObject mainCardSlotPrefab;
        [SerializeField] private GameObject sideCardSlotPrefab;
        [SerializeField] private Transform mainDeckInventory;
        [SerializeField] private Transform sideDeckInventory;

        [SerializeField] private TextMeshProUGUI mainDeckIndicator;
        [SerializeField] private TextMeshProUGUI sideDeckIndicator;

        [Header("Overflow Management")]
        [SerializeField] private GameObject closeButton;
        [SerializeField] private GameObject deleteButton;

        private IObjectPool<CardSlotView> mainDeckPool;
        private IObjectPool<CardSlotView> sideDeckPool;
        private List<CardSlotView> activeMainDeckSlots = new List<CardSlotView>();
        private List<CardSlotView> activeSideDeckSlots = new List<CardSlotView>();

        public override void OnInitialized()
        {
            uiRoot.SetActive(false);
            cardInspectorView.GetStatusEffectData = commander.GetStatusEffectData;

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
                defaultCapacity : Constant.BASE_MAX_DECK_CARD_TYPE_COUNT * Constant.BASE_MAX_COPIES_PER_CARD,
                maxSize : 100
            );

            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus.Subscribe<DeckChanged>(OnDeckChanged);
            eventBus.Subscribe<CardObtained>(OnCardObtained);
            eventBus.Subscribe<CardRemoved>(OnCardRemoved);
            eventBus.Subscribe<CardRemoveRequested>(OnCardRemoveRequested);

            sortingSettingView.SetOnButtonPressed(ChangeSortingState);
            filteringSettingView.SetOnButtonPressed(ToggleAttributeFilteringState, ToggleTypeFilteringState, ToggleCostFilteringState);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus?.Unsubscribe<DeckChanged>(OnDeckChanged);
            eventBus?.Unsubscribe<CardObtained>(OnCardObtained);
            eventBus?.Unsubscribe<CardRemoved>(OnCardRemoved);
            eventBus?.Unsubscribe<CardRemoveRequested>(OnCardRemoveRequested);
        }

        public void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            InitializeView();
            playerDeck = payload.Deck;
        }

        public void OnDeckChanged(DeckChanged payload)
        {
            playerDeck = payload.Deck;
            DrawView();
        }

        public void OnCardObtained(CardObtained payload)
        {
            if (uiRoot.activeSelf)
            {
                DrawView();
            }
        }

        public void OnCardRemoved(CardRemoved payload)
        {
            if (uiRoot.activeSelf)
            {
                DrawView();
            }
        }

        public void OnCardRemoveRequested(CardRemoveRequested payload)
        {
            if (!uiRoot.activeSelf)
            {
                uiRoot.SetActive(true);
                DrawView();
            }

            closeButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(true);
        }

        public void OnDeleteButtonClicked()
        {
            if (focusedCard == null)
            {
                Debug.LogWarning("[DeckInventoryView] 삭제할 카드가 선택되지 않았습니다.");
                return;
            }

            commander.RemoveAllCardOfData(focusedCard.Data);

            focusedCard = null;
            focusedSlot?.OnUnfocus();
            focusedSlot = null;

            if (!commander.IsDeckOverflowed)
            {
                closeButton.SetActive(true);
                deleteButton.SetActive(false);
            }
        }

        private CardSlotView CreateMainCardSlot()
        {
            var cardSlotView = Instantiate(mainCardSlotPrefab, mainDeckInventory);
            cardSlotView.SetActive(false);
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
            DrawView();
            uiRoot.SetActive(true);

            if (commander != null && !commander.IsDeckOverflowed)
            {
                closeButton.SetActive(true);
                deleteButton.SetActive(false);
            }
        }

        private void DrawView()
        {
            ClearActiveSlots();
            focusedSlot = null;

            sortingSettingView.SetState(deckSorter.SortingState);
            filteringSettingView.SetState((deckSorter.FilteringAttributes, deckSorter.FilteringType, deckSorter.FilteringCost));

            var mainDeckSnapshot = new Dictionary<CardData, List<Card>>(playerDeck.MainDeck);
            var sideDeckSnapshot = new Dictionary<CardData, List<Card>>(playerDeck.SideDeck);

            DrawInventory(mainDeckSnapshot, DeckType.MAIN_DECK);
            DrawInventory(sideDeckSnapshot, DeckType.SIDE_DECK);
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
                    mainDeckIndicator.text = $"<style=\"DeckName\">전투 덱</style> <style=\"DeckSubscription\">(전투 덱 카드 종류 : {deck.Count}/{Constant.MAX_MAIN_DECK_CARD_TYPE_COUNT})</style>";
                    break;
                case DeckType.SIDE_DECK:
                    sideDeckIndicator.text = $"<style=\"DeckName\">보조 덱</style> <style=\"DeckSubscription\">(보조 덱 카드 종류 : {GetSideDeckDescription(playerDeck.OwingCardVariety, playerDeck.MaxCardVariety)})</style>";
                    break;
                default:
                    throw new InvalidCastException($"[DeckInventoryView] {type} is not valid.");
            }

            string GetSideDeckDescription(int currentCardVariety, int maxCardVairety)
            {
                return currentCardVariety > maxCardVairety ? $"<color=#FF0000>{currentCardVariety}/{maxCardVairety}</color>" : $"{currentCardVariety}/{maxCardVairety}";
            }
        }
        
        public void NotifySlotClicked(CardSlotView slotView)
        {
            focusedSlot?.OnUnfocus();
            
            focusedSlot = slotView;
            focusedCard = slotView.CurrentCard; 
            
            focusedSlot.OnFocused();

            cardInspectorView.VisualizeSelectedSlot(slotView.CurrentCard);
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