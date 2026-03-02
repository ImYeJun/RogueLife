using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class DeckInventoryView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
    {
        [SerializeField] private UnityEvent<Card> OnSlotClicked;

        [SerializeField] private GameObject cardSlotPrefab;
        [SerializeField] private Transform mainDeckInventory;
        [SerializeField] private Transform sideDeckInventory;

        [SerializeField] private TextMeshProUGUI mainDeckIndicator;
        [SerializeField] private TextMeshProUGUI sideDeckIndicator;

        private IObjectPool<CardSlotView> mainDeckPool;
        private IObjectPool<CardSlotView> sideDeckPool;

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
        }

        private CardSlotView CreateMainCardSlot()
        {
            var cardSlotView = Instantiate(cardSlotPrefab);
            cardSlotView.transform.SetParent(mainDeckInventory);

            return cardSlotView.GetComponent<CardSlotView>();
        }
        private CardSlotView CreateSideCardSlot()
        {
            var cardSlotView = Instantiate(cardSlotPrefab);
            cardSlotView.transform.SetParent(sideDeckInventory);

            return cardSlotView.GetComponent<CardSlotView>();
        }
        private void GetDeckCardSlot(CardSlotView view)
        {
            view.gameObject.SetActive(true);
        }
        private void ReturnDeckCardSlot(CardSlotView view)
        {
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
            DrawInventory(payload.Deck.MainDeck, DeckType.MAIN_DECK);
            DrawInventory(payload.Deck.SideDeck, DeckType.SIDE_DECK);
        }
        private void DrawInventory(IReadOnlyDictionary<CardData, List<Card>> deck, DeckType type)
        {
            IObjectPool<CardSlotView> pool = type switch
            {
                DeckType.MAIN_DECK => mainDeckPool,
                DeckType.SIDE_DECK => sideDeckPool,
                _ => throw new InvalidCastException($"[DeckInventoryView] {type} is not valid.")
            };
            foreach (var pair in deck)
            {
                foreach (var card in pair.Value)
                {
                    var slot = pool.Get();

                    slot.Activate(card, OnSlotClicked);
                }
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
    }
}
