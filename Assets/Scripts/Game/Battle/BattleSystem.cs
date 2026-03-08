using System;
using System.Collections.Generic;
using System.Linq;
using Battle.BattleResultCommands;
using Battle.StartEffects;
using UnityEditor.Experimental.GraphView;

public class BattleSystem : IFieldBattleSystem
{
    private BattleContext context;
    private BattleEventBus eventBus;
    private BattleScheduler scheduler;
    private BattleStartEffectSystem startEffectSystem;
    private BattleActionPipeline pipeline;
    private BattlePhase phase;
    private BattleActionCost actionCost;
    private BattlePlayerContainer playerContainer;
    private BattleBelongingsBag belongingsBag;
    private BattleDeckSystem deckSystem;
    private BattleEnemySystem enemySystem;

    public BattleSystem(Random random, IBattleCardDatabase cardDatabase, IBattleBattleStatusEffectDatabase battleStatusEffectDatabase)
    {
        eventBus = new BattleEventBus();
        scheduler = new BattleScheduler(ExitBattle);
        startEffectSystem = new BattleStartEffectSystem();
        pipeline = new BattleActionPipeline();
        phase = new BattlePhase();
        playerContainer = new BattlePlayerContainer();
        belongingsBag = new BattleBelongingsBag();
        actionCost = new BattleActionCost();
        deckSystem = new BattleDeckSystem();
        enemySystem = new BattleEnemySystem();

        context = new BattleContext(
            random : random,
            eventBus : eventBus,
            cardDatabase : cardDatabase,
            battleStatusEffectDatabase : battleStatusEffectDatabase,
            battleScheduler : scheduler,
            actionScheduler : pipeline,
            actionObserverHub : pipeline,
            phase : phase,
            playerContainer : playerContainer,
            belongingsBag : belongingsBag,
            actionCost : actionCost,
            actionCostHistory : actionCost.History,
            deckSystem : deckSystem,
            battleDeckHistory : deckSystem.History,
            drawDeck : deckSystem[BattleDeckType.DRAW],
            handDeck : deckSystem[BattleDeckType.HAND],
            graveDeck : deckSystem[BattleDeckType.GRAVE],
            enemySystem : enemySystem,
            enemyHistory : enemySystem.History
        );

        scheduler.SetContext(context);
        startEffectSystem.SetContext(context);
        pipeline.SetContext(context);
        phase.SetContext(context);
        deckSystem.SetContext(context);
        enemySystem.SetContext(context);

        startEffectSystem.SubscribeEventBus(eventBus);
        pipeline.SubscribeEventBus(eventBus);
        phase.SubscribeEventBus(eventBus);
        actionCost.SubscribeEventBus(eventBus);
        actionCost.History.SubscribeEventBus(eventBus);
        deckSystem.SubscribeEventBus(eventBus);
        deckSystem.History.SubscribeEventBus(eventBus);
        playerContainer.SubscribeEventBus(eventBus);
        enemySystem.SubscribeEventBus(eventBus);
        enemySystem.History.SubscribeEventBus(eventBus);
    }

    //TODO Refactor this nullalbe attributes
    public event Action<BattleResultCommand> OnBattleExit;
    private EnemyTier mainEnemyTier;
    private IBattleEntryActionCost fieldActionCost;

    public void EngageBattle(IBattleHealth battleHealth, IBattleEntryActionCost actionCost, IBattleEntryDeck deck, IBattleEntryBelongingsBag entrybelongingsBag, List<EnemyDataSlot> engagingEnemiesDataSlot,  Action<BattleResultCommand> battleExit)
    {
        var mainEnemyData = engagingEnemiesDataSlot.OrderByDescending(slot => slot.Entity.Tier).First().Entity;
        mainEnemyTier = mainEnemyData.Tier;

        OnBattleExit = battleExit;
        fieldActionCost = actionCost;

        int startPhaseCount = mainEnemyData.Tier switch
        {
            EnemyTier.NORMAL => Constant.NORMAL_ENEMY_START_PHASE_COUNT,
            EnemyTier.ELITE => Constant.ELITE_ENEMY_START_PHASE_COUNT,
            EnemyTier.BOSS => Constant.BOSS_ENEMY_START_PHASE_COUNT,
            _ => throw new InvalidOperationException($"[BattleSystem] {mainEnemyData.Tier} is not supported for determining start phase count.")
        };

        int maxActionCost = actionCost.CurrentMaxActionCost;
        int fisrtTurnDrawCount = Constant.BASE_FIRST_TURN_DRAW_COUNT;
        int turnStartDrawCount = Constant.BASE_START_TURN_DRAW_COUNT;
        List<Card> startDrawDeck = deck.GetClonedMainDeck(isForBattleStart : true).Values.SelectMany(sel => sel).ToList();

        BattlePlayer battlePlayer = new BattlePlayer(context, battleHealth);
        playerContainer.OnEngageBattle(battlePlayer);

        List<BattleBelongings> battleBelongings = entrybelongingsBag.GetBattleBelongings(battlePlayer);
        belongingsBag.OnEngageBattle(battleBelongings, context);

        List<BattleEnemy> enemies = new List<BattleEnemy>();
        foreach (var dataSlot in engagingEnemiesDataSlot)
        {
            enemies.Add(new BattleEnemy(context, dataSlot.Entity));
        }

        scheduler.StartBattle(startPhaseCount, maxActionCost, fisrtTurnDrawCount, turnStartDrawCount, startDrawDeck, battlePlayer, enemies);
    }

    public void ExitBattle(BattleResult result)
    {
        BattleResultCommand resultCommand = result switch
        {
            BattleResult.PLAYER_SPECIAL_CARD_WIN => new 
                CompositeCommand(mainEnemyTier, new List<BattleResultCommand>(){ 
                    new ObtainCardCommand(mainEnemyTier),
                    new ObtainBelongingsCommand(mainEnemyTier),
                    new RequestNextNodeSelectionCommand(mainEnemyTier)
                }),
            BattleResult.PLAYER_ANNIHILATE_WIN => new 
                CompositeCommand(mainEnemyTier, new List<BattleResultCommand>(){ 
                    new ObtainCardCommand(mainEnemyTier),
                    new ObtainBelongingsCommand(mainEnemyTier),
                    new RequestNextNodeSelectionCommand(mainEnemyTier)
                }),
            BattleResult.ALL_PHASE_END => new CompositeCommand(mainEnemyTier, new List<BattleResultCommand>(){ 
                        new ReceiveDamageCommand(mainEnemyTier),
                        new RequestNextNodeSelectionCommand(mainEnemyTier)
                    }),
            BattleResult.PLAYER_DIED => new PlayerDiedCommand(mainEnemyTier),
            BattleResult.OUT_OF_MY_WAY => new OutOfMyWayCommand(mainEnemyTier),
            _ => throw new InvalidOperationException($"[BattleSystem] {result} is not valid to generate resultCommand.")
        };
        
        fieldActionCost?.OnBattleEnd();
        OnBattleExit?.Invoke(resultCommand);
        OnBattleExit = null;
        fieldActionCost = null;
    }

    public void AddBattleStartEffect(BattleStartEffect effect)
    {
        startEffectSystem.AddEffect(effect);
    }
}