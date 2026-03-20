#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ViewEvent.BattleView;
using Field.Deck.Observers;

public class BattleDeckSystem : IBattleDeckSystemContext, IBattleEventObserveService
{
    private BattleContext context;
    private IBattleViewEventPublisher viewEventPublisher;
    private BattleDeckHistory history;
    private bool isFirstTurn;
    private int firstTurnDrawCount;
    private int turnStartDrawCount;
    
    private Dictionary<BattleDeckType, BattleDeck> deckMap = new Dictionary<BattleDeckType, BattleDeck>
    {
        { BattleDeckType.DRAW, new BattleDeck() },
        { BattleDeckType.HAND, new BattleDeck() },
        { BattleDeckType.GRAVE, new BattleDeck() }
    };

    private HashSet<Card> activeTriggeringCards = new HashSet<Card>();
    
    private HashSet<IDeckObserver> handDeckObservers = new HashSet<IDeckObserver>();

    public BattleDeckSystem(IBattleViewEventPublisher viewEventPublisher)
    {
        this.viewEventPublisher = viewEventPublisher;
        history = new BattleDeckHistory();
    }

    public BattleDeck this[BattleDeckType type]
    {
        get
        {
            if (!deckMap.ContainsKey(type)) { throw new ArgumentOutOfRangeException($"Battle Deck System doesn't have the given type deck {type}");}
            return deckMap[type];
        }
    }
    public BattleDeckHistory History { get => history; }

    public void SetContext(BattleContext context) { 
        this.context = context;
    }

    public void MoveCard(Card card, BattleDeckType destination)
    {
        var sourceDecks = deckMap.Values.Where(deck => deck.HasCard(card));
        if (sourceDecks.Count() == 0) { throw new InvalidOperationException($"[BattleDeckSystem/MoveCard] The given card isn't located in any deck. (cardName : {card?.CurrentName})"); }
        if (sourceDecks.Count() >= 2) { throw new InvalidOperationException($"[BattleDeckSystem/MoveCard] The given card is located in more than one deck. (cardName : {card?.CurrentName})");}

        var sourceDeck = sourceDecks.First();
        var destinationDeck = deckMap[destination];
        if (sourceDeck == destinationDeck) { throw new InvalidOperationException($"[BattleDeckSystem/MoveCard] Source deck and destination deck cannot be the same. (deckType : {destination})"); }

        if (destinationDeck == deckMap[BattleDeckType.HAND] && destinationDeck.Count >= Constant.BASE_MAX_HAND_ZONE_CARD_COUNT)
        {
            return;
        }

        sourceDeck.RemoveCard(card);
        destinationDeck.AddCard(card);

        if (sourceDeck == deckMap[BattleDeckType.HAND])
        {
            foreach (var observer in handDeckObservers)
            {
                observer.OnCardRemoved(card);
            }
        }

        if (destinationDeck == deckMap[BattleDeckType.HAND])
        {
            foreach (var observer in handDeckObservers)
            {
                observer.OnCardEquipped(card);
            }
        }

        if (sourceDeck == deckMap[BattleDeckType.DRAW] && destinationDeck == deckMap[BattleDeckType.HAND])
        {
            card.OnDraw(context);
            viewEventPublisher.Publish(new CardDrawed(viewEventPublisher.GetNextSequenceId(), card));
        }
        if (sourceDeck == deckMap[BattleDeckType.HAND])
        {
            if (destinationDeck == deckMap[BattleDeckType.GRAVE])
            {
                history.RecordGravedCard(card);
            }

            viewEventPublisher.Publish(new CardDiscarded(viewEventPublisher.GetNextSequenceId(), card, destination));
        }
        if (sourceDeck == deckMap[BattleDeckType.GRAVE] && destinationDeck == deckMap[BattleDeckType.DRAW])
        {
            context.ActionScheduler.Enqueue(new ApplyReflectEffectOnCard(card));
            viewEventPublisher.Publish(new CardRestored(viewEventPublisher.GetNextSequenceId(), card));
        }
    }

    public Card? RequestDrawingCard(System.Random random, CardRarity rarity, CardAttribute attribute, CardType type)
    {
        return deckMap[BattleDeckType.DRAW].GetRandomCard(random, rarity, attribute, type);
    }

    public void AddActiveTriggerCard(Card card)
    {
        if (!activeTriggeringCards.Add(card))
        {
            throw new InvalidOperationException($"[BattleDeckSystem/AddActiveTriggerCard] The card is already in the active triggering list. (cardName : {card?.CurrentName})");
        }
    }

    public void RemoveActiveTriggerCard(Card card)
    {
        if (!activeTriggeringCards.Remove(card))
        {
            throw new InvalidOperationException($"[BattleDeckSystem/RemoveActiveTriggerCard] The card is not in the active triggering list. (cardName : {card?.CurrentName})");
        }
        
        viewEventPublisher.Publish(new CardTriggerResolved(viewEventPublisher.GetNextSequenceId(), card));
    }

    public void RegisterHandDeckObserver(IDeckObserver observer)
    {
        if (handDeckObservers.Contains(observer))
        {
            Debug.Log("[BattleDeckSystem] Hand Deck already has the observer");
            return;
        }

        handDeckObservers.Add(observer);
        observer.OnStartObserving(deckMap[BattleDeckType.HAND].GetCards().ToList());
    }

    public void UnregisterHandDeckObserver(IDeckObserver observer)
    {
        if (!handDeckObservers.Contains(observer))
        {
            Debug.Log("[BattleDeckSystem] Hand Deck doesn't have the observer");
            return;
        }

        observer.OnStopObserving(deckMap[BattleDeckType.HAND].GetCards().ToList());
        handDeckObservers.Remove(observer);
    }

    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        eventBus.Subscribe<BattleStartEvent>(Initiate);
        eventBus.Subscribe<PlayerTurnStartBattleEvent>(StartTurnDraw);
        eventBus.Subscribe<BattleEndBattleEvent>(OnBattleEnd);
    }
    
    public void Initiate(BattleStartEvent payload)
    {
        firstTurnDrawCount = payload.FirstTurnDrawCount;
        turnStartDrawCount = payload.TurnStartDrawCount;
        isFirstTurn = true;

        foreach (var pair in deckMap)
        {
            pair.Value.Clear();

            if (pair.Key == BattleDeckType.DRAW)
            {
                pair.Value.SetDeck(payload.StartDrawDeck);
            }
        }
        
        activeTriggeringCards.Clear();

        viewEventPublisher.Publish(new InitialDeckSettled(
            sequenceId : viewEventPublisher.GetNextSequenceId(),
            handDeck : this[BattleDeckType.DRAW],
            drawDeck : this[BattleDeckType.DRAW],
            graveDeck : this[BattleDeckType.GRAVE]
            ));
    }
    
    public void StartTurnDraw(PlayerTurnStartBattleEvent payload)
    {
        int acutalDrawAmount = isFirstTurn ? firstTurnDrawCount : turnStartDrawCount;
        isFirstTurn = false;

        if (deckMap[BattleDeckType.DRAW].Count < acutalDrawAmount)
        {
            ReviveGraveCards();
        }

        int totalAvailable = deckMap[BattleDeckType.DRAW].Count + deckMap[BattleDeckType.GRAVE].Count;
        acutalDrawAmount = Mathf.Min(acutalDrawAmount, totalAvailable);

        for (int i = 0; i < acutalDrawAmount; i++)
        {
            context.ActionScheduler.Enqueue(new RequestDrawCardBattleAction(CardRarity.ANY, CardAttribute.ANY, CardType.ANY, Guid.NewGuid()));
        }
    }
    
    public void OnBattleEnd(BattleEndBattleEvent payload)
    {
        handDeckObservers.Clear();
    }
    
    public void ReviveGraveCards(bool insertFront = false)
    {
        var graveDeck = deckMap[BattleDeckType.GRAVE];

        if (insertFront)
        {
            for (int i = graveDeck.Count - 1; i >= 0; i--)
            {
                context.ActionScheduler.EnqueueFront(new MoveCardToDeckBattleAction(graveDeck[i], BattleDeckType.DRAW));
            }
        }
        else
        {
            for (int i = graveDeck.Count - 1; i >= 0; i--)
            {
                context.ActionScheduler.Enqueue(new MoveCardToDeckBattleAction(graveDeck[i], BattleDeckType.DRAW));
            }
        }
    }

    public void RequestUseCard(Card card, bool isFreeUse)
    {
        viewEventPublisher.Publish(new UseCardRequested(viewEventPublisher.GetNextSequenceId(), card, isFreeUse));
    }

    public void RequestTriggerCard(Card card, bool isReflection)
    {
        viewEventPublisher.Publish(new TriggerCardRequested(viewEventPublisher.GetNextSequenceId(), card, isReflection));
    }
}