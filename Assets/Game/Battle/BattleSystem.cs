using System;
using System.Collections.Generic;
using System.Linq;

public class BattleSystem : IFieldBattleSystem
{
    private BattleContext context;
    private BattleEventBus eventBus;
    private BattleScheduler scheduler;
    private BattleActionPipeline pipeline;
    private BattlePhase phase;
    private BattleActionCost acionCost;
    private BattlePlayerContainer playerContainer;
    private BattleDeckSystem deckSystem;
    private BattleEnemySystem enemySystem;

    public BattleSystem(Random random)
    {
        eventBus = new BattleEventBus();
        scheduler = new BattleScheduler(ExitBattle);
        pipeline = new BattleActionPipeline();
        phase = new BattlePhase();
        playerContainer = new BattlePlayerContainer();
        acionCost = new BattleActionCost();
        deckSystem = new BattleDeckSystem();
        enemySystem = new BattleEnemySystem();

        context = new BattleContext(
            random : random,
            eventBus : eventBus,
            battleScheduler : scheduler,
            actionScheduler : pipeline,
            actionObserverHub : pipeline,
            phase : phase,
            playerContainer : playerContainer,
            actionCost : acionCost,
            actionCostHistory : acionCost.History,
            deckSystem : deckSystem,
            cardPlayHistory : deckSystem.History,
            drawDeck : deckSystem[BattleDeckType.DRAW],
            handDeck : deckSystem[BattleDeckType.HAND],
            graveDeck : deckSystem[BattleDeckType.GRAVE],
            enemySystem : enemySystem,
            enemyHistory : enemySystem.History
        );

        scheduler.SetContext(context);
        pipeline.SetContext(context);
        phase.SetContext(context);
        deckSystem.SetContext(context);
        enemySystem.SetContext(context);

        eventBus.Subscribe(phase);
        eventBus.Subscribe(acionCost);
        eventBus.Subscribe(acionCost.History);
        eventBus.Subscribe(deckSystem);
        eventBus.Subscribe(deckSystem.History);
        eventBus.Subscribe(playerContainer);
        eventBus.Subscribe(enemySystem);
        eventBus.Subscribe(enemySystem.History);
    }

    public event Action<BattleResult> OnBattleExit;

    public void EngageBattle(IBattleHealth battleHealth, IBattleEntryActionCost actionCost, IBattleEntryDeck deck, IBattleEntryBelongingsBag belongingsBag, List<EnemyDataSlot> engagingEnemiesDataSlot,  Action<BattleResult> battleExit)
    {
        var mainEnemyData = engagingEnemiesDataSlot.OrderByDescending(slot => slot.Data.Tier).First().Data;
        int startPhaseCount = mainEnemyData.Tier switch
        {
            EnemyTier.NORMAL => Constant.NORMAL_ENEMY_START_PHASE_COUNT,
            EnemyTier.ELITE => Constant.ELITE_ENEMY_START_PHASE_COUNT,
            EnemyTier.BOSS => Constant.BOSS_ENEMY_START_PHASE_COUNT,
            _ => throw new InvalidOperationException($"[BattleSystem] {mainEnemyData.Tier} is not supported for determining start phase count.")
        };

        int maxActionCost = actionCost.MaxActionCost;
        int fisrtTurnDrawCount = Constant.BASE_FIRST_TURN_DRAW_COUNT;
        int turnStartDrawCount = Constant.BASE_START_TURN_DRAW_COUNT;
        List<Card> startDrawDeck = deck.GetClonedMainDeck().Values.SelectMany(sel => sel).ToList();

        BattlePlayer battlePlayer = new BattlePlayer(context, battleHealth);
        List<BattleBelongings> battleBelongingsBag = belongingsBag.GetBattleBelongings(battlePlayer);
        foreach (var battleBelongings in battleBelongingsBag) { eventBus.Subscribe(battleBelongings.BehaviourInstance); }
        battlePlayer.SetBelongings(battleBelongingsBag);

        List<BattleEnemy> enemies = new List<BattleEnemy>();
        foreach (var dataSlot in engagingEnemiesDataSlot)
        {
            enemies.Add(new BattleEnemy(context, dataSlot.Data));
        }

        scheduler.StartBattle(startPhaseCount, maxActionCost, fisrtTurnDrawCount, turnStartDrawCount, startDrawDeck, battlePlayer, enemies);
    }

    public void ExitBattle(BattleResult result)
    {
        var battleBelongings = playerContainer.Player.Belongings;

        foreach (var belongings in battleBelongings)
        {
            eventBus.Unsubscribe(belongings.BehaviourInstance);
        }
        
        OnBattleExit?.Invoke(result);
    }

    public void RegisterBattleStartBuff(BattleStatusEffect buff, FieldEffectDuration duration)
    {
        throw new NotImplementedException();
        //TODO 구체 데이터 만들면서 구현하기
    }
}