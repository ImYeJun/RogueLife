using System;
using UnityEngine;

public class BattleContext
{
    private System.Random random;
    private IBattleScheduler battleScheduler;
    private IBattleEventBus eventBus;
    private IBattleActionScheduler actionScheduler;
    private IBattleActionObserverHub actionObserverHub;
    private IBattlePhaseContext phase;
    private IBattlePlayerContainerContext playerContainer;
    private IBattleActionCost actionCost;
    private IBattleActionCostHistoryContext actionCostHistory;
    private IBattleDeckHistoryContext cardPlayHistory;
    private IBattleDeckSystemContext deckSystem;
    private IDrawDeckContext drawDeck;
    private IHandDeckContext handDeck;
    private IGraveDeckContext graveDeck;
    private IBattleEnemySystemContext enemySystem;
    private IBattleEnemyHistoryContext enemyHistory;

    public System.Random Random { get => random; }
    public IBattleScheduler BattleScheduler { get => battleScheduler; }
    public IBattleEventBus EventBus { get => eventBus; }
    public IBattleActionScheduler ActionScheduler { get => actionScheduler; }
    public IBattleActionObserverHub ActionObserverHub { get => actionObserverHub; }
    public IBattlePhaseContext Phase { get => phase; }
    public IBattlePlayerContainerContext PlayerContainer { get => playerContainer; }
    public IBattleActionCost ActionCost { get => actionCost; }
    public IBattleActionCostHistoryContext ActionCostHistory { get => actionCostHistory; }
    public IBattleDeckHistoryContext CardPlayHistory { get => cardPlayHistory; }
    public IBattleDeckSystemContext DeckSystem { get => deckSystem; }
    public IDrawDeckContext DrawDeck { get => drawDeck; }
    public IHandDeckContext HandDeck { get => handDeck; }
    public IGraveDeckContext GraveDeck { get => graveDeck; }
    public IBattleEnemySystemContext EnemySystem { get => enemySystem; }
    public IBattleEnemyHistoryContext EnemyHistory { get => enemyHistory; }
}