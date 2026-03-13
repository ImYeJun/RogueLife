#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ViewEvent.BattleView;

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
        if (sourceDecks.Count() == 0) { throw new InvalidOperationException($"The given card isn't located in any deck. (cardName : {card?.CurrentName})"); }
        if (sourceDecks.Count() >= 2) { throw new InvalidOperationException($"The given card is located in more than one deck. (cardName : {card?.CurrentName})");}

        var sourceDeck = sourceDecks.First();
        var destinationDeck = deckMap[destination];
        if (sourceDeck == destinationDeck) { throw new InvalidOperationException($"Source deck and destination deck cannot be the same. (deckType : {destination})"); }

        sourceDeck.RemoveCard(card);
        destinationDeck.AddCard(card);

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

    public void NullifyCardUseOnStunned(TryUseCardBattleAction tryUseCardBattleAction, BattleContext context)
    {
        var player =  context.PlayerContainer.Player;

        if (player.CurrentCondition.HasFlag(BattleEntityCondition.STUNNED))
        {
            tryUseCardBattleAction.Nullify();
        }
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

        context.ActionObserverHub.SubscribeActionModifier<TryUseCardBattleAction>(NullifyCardUseOnStunned);

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
        context.ActionObserverHub.UnsubscribeActionModifier<TryUseCardBattleAction>(NullifyCardUseOnStunned);
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
                context.ActionScheduler.Enqueue(new    MoveCardToDeckBattleAction(graveDeck[i], BattleDeckType.DRAW));
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