using System;
using UnityEngine;

public class BattleContext
{
    private System.Random random;
    private IBattleEventBus eventBus;
    private IBattleScheduler battleScheduler;
    private IBattleActionScheduler actionScheduler;
    private IBattleActionObserverHub actionObserverHub;
    private IBattlePhaseContext phase;
    private IBattlePlayerContainerContext playerContainer;
    private IBattleActionCost actionCost;
    private IBattleActionCostHistoryContext actionCostHistory;
    private IBattleDeckSystemContext deckSystem;
    private IBattleDeckHistoryContext battleDeckHistory;
    private IDrawDeckContext drawDeck;
    private IHandDeckContext handDeck;
    private IGraveDeckContext graveDeck;
    private IBattleEnemySystemContext enemySystem;
    private IBattleEnemyHistoryContext enemyHistory;

    public BattleContext(System.Random random, IBattleEventBus eventBus, IBattleScheduler battleScheduler, IBattleActionScheduler actionScheduler, IBattleActionObserverHub actionObserverHub, IBattlePhaseContext phase, IBattlePlayerContainerContext playerContainer, IBattleActionCost actionCost, IBattleActionCostHistoryContext actionCostHistory, IBattleDeckSystemContext deckSystem, IBattleDeckHistoryContext cardPlayHistory, IDrawDeckContext drawDeck, IHandDeckContext handDeck, IGraveDeckContext graveDeck, IBattleEnemySystemContext enemySystem, IBattleEnemyHistoryContext enemyHistory)
    {
        this.random = random;
        this.eventBus = eventBus;
        this.battleScheduler = battleScheduler;
        this.actionScheduler = actionScheduler;
        this.actionObserverHub = actionObserverHub;
        this.phase = phase;
        this.playerContainer = playerContainer;
        this.actionCost = actionCost;
        this.actionCostHistory = actionCostHistory;
        this.deckSystem = deckSystem;
        this.battleDeckHistory = cardPlayHistory;
        this.drawDeck = drawDeck;
        this.handDeck = handDeck;
        this.graveDeck = graveDeck;
        this.enemySystem = enemySystem;
        this.enemyHistory = enemyHistory;
    }

    public System.Random Random { get => random; }
    public IBattleEventBus EventBus { get => eventBus; }
    public IBattleScheduler BattleScheduler { get => battleScheduler; }
    public IBattleActionScheduler ActionScheduler { get => actionScheduler; }
    public IBattleActionObserverHub ActionObserverHub { get => actionObserverHub; }
    public IBattlePhaseContext Phase { get => phase; }
    public IBattlePlayerContainerContext PlayerContainer { get => playerContainer; }
    public IBattleActionCost ActionCost { get => actionCost; }
    public IBattleActionCostHistoryContext ActionCostHistory { get => actionCostHistory; }
    public IBattleDeckSystemContext DeckSystem { get => deckSystem; }
    public IBattleDeckHistoryContext BattleDeckHistory { get => battleDeckHistory; }
    public IDrawDeckContext DrawDeck { get => drawDeck; }
    public IHandDeckContext HandDeck { get => handDeck; }
    public IGraveDeckContext GraveDeck { get => graveDeck; }
    public IBattleEnemySystemContext EnemySystem { get => enemySystem; }
    public IBattleEnemyHistoryContext EnemyHistory { get => enemyHistory; }
}