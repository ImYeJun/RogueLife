using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDeckSystem : IBattleDeckSystemContext, IBattleEventObserver, IBattleActionModifier
{
    private BattleContext context;
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
    public BattleDeck this[BattleDeckType type]
    {
        get
        {
            if (!deckMap.ContainsKey(type)) { throw new ArgumentOutOfRangeException($"Battle Deck System doesn't have the given type deck {type}");}
            return deckMap[type];
        }
    }
    public BattleDeckHistory History { get => history; }

    public void SetContext(BattleContext context) { this.context = context; }

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
        }
        if (sourceDeck == deckMap[BattleDeckType.HAND] && destinationDeck == deckMap[BattleDeckType.GRAVE])
        {
            history.RecordGravedCard(card);
        }
        if (sourceDeck == deckMap[BattleDeckType.GRAVE] && destinationDeck == deckMap[BattleDeckType.DRAW])
        {
            card.ApplyReflection();
        }
    }

    public Card RequestDrawingCard(System.Random random, CardAttribute attribute, CardType type)
    {
        return deckMap[BattleDeckType.DRAW].GetRandomCard(random, attribute, type);
    }

    public void ModifyAction(IBattleAction action, BattleContext context)
    {
        if (action is TryUseCardBattleAction tryUseCardBattleActionv)
        {
            var player =  context.PlayerContainer.Player;
            if (player.CurrentCondition.HasFlag(BattleEntityCondition.STUNNED))
            {
                tryUseCardBattleActionv.Nullify();
            }
        }
    }

    public void OnBattleEvent(BattleEvent battleEvent)
    {
        if (battleEvent is BattleStartEvent payload)
        {
            firstTurnDrawCount = payload.FisrtTurnDrawCount;
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

            context.ActionObserverHub.SubscribeActionModifier(this);
        }

        if (battleEvent is PlayerTurnStartBattleEvent)
        {
            int acutalDrawAmount = isFirstTurn ? firstTurnDrawCount : turnStartDrawCount;
            isFirstTurn = false; 

            if (deckMap[BattleDeckType.DRAW].Count < acutalDrawAmount)
            {
                ReviveGraveCards();
            }

            acutalDrawAmount = Mathf.Min(acutalDrawAmount, deckMap[BattleDeckType.DRAW].Count);

            for (int i = 0; i < acutalDrawAmount; i++)
            {
                context.ActionScheduler.Enqueue(new RequestDrawCardBattleAction(CardAttribute.ANY, CardType.ANY, Guid.NewGuid()));
            }
        }

        if (battleEvent is BattleEndBattleEvent)
        {
            context.ActionObserverHub.UnsubscribeActionModifier(this);
        }
    }

    private void ReviveGraveCards()
    {
        var graveDeck = deckMap[BattleDeckType.GRAVE];
        for (int i = graveDeck.Count - 1; i >= 0; i--)
        {
            context.ActionScheduler.Enqueue(new MoveCardToDeckBattleAction(graveDeck[i], BattleDeckType.DRAW));
        }
    }
}