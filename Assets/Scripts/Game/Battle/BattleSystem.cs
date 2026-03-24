using System;
using System.Collections.Generic;
using System.Linq;
using Battle.BattleResultCommands;
using Battle.StartEffects;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using ViewEvent.BattleView;

public partial class BattleSystem : IFieldBattleSystem, IBattleViewCommander
{
    private BattleViewEventBus viewEventBus;

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
        viewEventBus = new BattleViewEventBus();

        eventBus = new BattleEventBus();
        scheduler = new BattleScheduler(ExitBattle, viewEventBus);
        startEffectSystem = new BattleStartEffectSystem();
        pipeline = new BattleActionPipeline();
        phase = new BattlePhase(viewEventBus);
        playerContainer = new BattlePlayerContainer(viewEventBus);
        belongingsBag = new BattleBelongingsBag(viewEventBus);
        actionCost = new BattleActionCost(viewEventBus);
        deckSystem = new BattleDeckSystem(viewEventBus);
        enemySystem = new BattleEnemySystem(viewEventBus);

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
        belongingsBag.SetContext(context);

        startEffectSystem.SubscribeEventBus(eventBus);
        pipeline.SubscribeEventBus(eventBus);
        phase.SubscribeEventBus(eventBus);
        actionCost.SubscribeEventBus(eventBus);
        actionCost.History.SubscribeEventBus(eventBus);
        deckSystem.SubscribeEventBus(eventBus);
        deckSystem.History.SubscribeEventBus(eventBus);
        playerContainer.SubscribeEventBus(eventBus);
        belongingsBag.SubscribeEventBus(eventBus);
        enemySystem.SubscribeEventBus(eventBus);
        enemySystem.History.SubscribeEventBus(eventBus);

        eventBus.Subscribe<CardEffectExecutedBattleEvent>(OnCardEffectExecuted);
        eventBus.Subscribe<EnemyActionExecutedBattleEvent>(OnEnemyActionExecuted);
        eventBus.Subscribe<BattleStatusEffectExecutedBattleEvent>(OnBattleStatusEffectExecuted);
        eventBus.Subscribe<CardCostChangedBattleEvent>(OnCardCostChanged);
        eventBus.Subscribe<CardReflectionChangedBattleEvent>(OnCardReflectionChanged);
    }

    public event Action<BattleResult> OnBattleExit;
    private EnemyData mainEnemyData;
    private IBattleEntryActionCost fieldActionCost;
    private BattleStartData? preparedStartData;

    public BattleViewEventBus ViewEventBus { get => viewEventBus; }

    public void EngageBattle(IBattleHealth battleHealth, IBattleEntryActionCost actionCost, IBattleEntryDeck deck, IBattleEntryBelongingsBag entrybelongingsBag, List<EnemyDataSlot> engagingEnemiesDataSlot, Action<BattleResult> battleExit, Action onEngage)
    {
        var mainEnemyEntity = engagingEnemiesDataSlot.OrderByDescending(slot => slot.Entity.Tier).First().Entity;
        mainEnemyData = mainEnemyEntity.Data;

        OnBattleExit = battleExit;
        this.fieldActionCost = actionCost;

        int startPhaseCount = mainEnemyEntity.Tier switch
        {
            EnemyTier.NORMAL => Constant.NORMAL_ENEMY_START_PHASE_COUNT,
            EnemyTier.ELITE => Constant.ELITE_ENEMY_START_PHASE_COUNT,
            EnemyTier.BOSS => Constant.BOSS_ENEMY_START_PHASE_COUNT,
            _ => throw new InvalidOperationException($"[BattleSystem/EngageBattle] {mainEnemyEntity.Tier} is not supported for determining start phase count.")
        };

        int maxActionCost = actionCost.CurrentMaxActionCost;
        int firstTurnDrawCount = Constant.BASE_FIRST_TURN_DRAW_COUNT;
        int turnStartDrawCount = Constant.BASE_START_TURN_DRAW_COUNT;
        List<Card> startDrawDeck = deck.GetClonedMainDeck(isForBattleStart : true).Values.SelectMany(sel => sel).ToList();

        BattlePlayer battlePlayer = new BattlePlayer(context, battleHealth);
        List<BattleBelongings> battleBelongings = entrybelongingsBag.GetBattleBelongings(battlePlayer);

        List<BattleEnemy> enemies = new List<BattleEnemy>();
        foreach (var dataSlot in engagingEnemiesDataSlot)
        {
            enemies.Add(new BattleEnemy(context, dataSlot.Entity));
        }

        preparedStartData = new BattleStartData(
            startPhaseCount, maxActionCost, firstTurnDrawCount, turnStartDrawCount, startDrawDeck, battlePlayer, battleBelongings, enemies
        );

        onEngage?.Invoke();
    }

    public void StartBattle()
    {
        if (preparedStartData is null)
        {
            UnityEngine.Debug.LogError("[BattleSystem] battle data is not prepared");
            return;
        }

        scheduler.StartBattle(preparedStartData.Value);
    }

    public void ExitBattle(BattleResultType result)
    {
        var mainEnemyTier = mainEnemyData.Tier;
        bool hasResolved = result is BattleResultType.PLAYER_SPECIAL_CARD_WIN or BattleResultType.PLAYER_ANNIHILATE_WIN;
        bool nextNodeSelectable = result is BattleResultType.PLAYER_SPECIAL_CARD_WIN or BattleResultType.PLAYER_ANNIHILATE_WIN or BattleResultType.ALL_PHASE_END;

        BattleResultCommand resultCommand = result switch
        {
            BattleResultType.PLAYER_SPECIAL_CARD_WIN => new 
                CompositeCommand(mainEnemyTier, new List<BattleResultCommand>(){ 
                    new ObtainCardCommand(mainEnemyTier),
                    new ObtainBelongingsCommand(mainEnemyTier),
                }),
            BattleResultType.PLAYER_ANNIHILATE_WIN => new 
                CompositeCommand(mainEnemyTier, new List<BattleResultCommand>(){ 
                    new ObtainCardCommand(mainEnemyTier),
                    new ObtainBelongingsCommand(mainEnemyTier),
                }),
            BattleResultType.ALL_PHASE_END => new CompositeCommand(mainEnemyTier, new List<BattleResultCommand>(){ 
                        new ReceiveDamageCommand(mainEnemyTier),
                    }),
            BattleResultType.PLAYER_DIED => new PlayerDiedCommand(mainEnemyTier),
            BattleResultType.OUT_OF_MY_WAY => new OutOfMyWayCommand(mainEnemyTier),
            _ => throw new InvalidOperationException($"[BattleSystem/ExitBattle] {result} is not valid to generate resultCommand.")
        };
        
        BattleResult battleResult = new BattleResult(resultCommand, hasResolved, mainEnemyData, nextNodeSelectable);

        fieldActionCost?.OnBattleEnd();
        OnBattleExit?.Invoke(battleResult);
        OnBattleExit = null;
        fieldActionCost = null;
        preparedStartData = null;

        viewEventBus.Publish(new BattleExited(viewEventBus.GetNextSequenceId()));
    }

    public void AddBattleStartEffect(BattleStartEffect effect)
    {
        startEffectSystem.AddEffect(effect);
    }

    public bool IsAbleToUseCard(Card card, CardTarget cardTarget)
    {
        return card.IsAbleToUse(context, cardTarget);
    }

    public void CancelActivation(Card card, bool isTriggering)
    {
        if (isTriggering)
        {
            viewEventBus.Publish(new CardTriggerResolved(viewEventBus.GetNextSequenceId(), card));
        }
        else
        {
            viewEventBus.Publish(new CardActivationCancelled(viewEventBus.GetNextSequenceId(), card));
        }
        
        pipeline.EnqueueFront(new NotifyCardExecutionCompletedBattleAction(card));
        pipeline.Resume();
    }

    public void UseCard(Card card, CardTarget cardTarget, bool isFreeUse)
    {
        var cardUseAction = new TryUseCardBattleAction(isFreeUse ? 0 : card.CurrentActionCost, card, cardTarget);
        
        pipeline.EnqueueFront(new NotifyCardExecutionCompletedBattleAction(card));
        pipeline.EnqueueFront(cardUseAction);
        pipeline.Resume();
    }

    public void TriggerCard(Card card, CardTarget cardTarget, bool isReflection)
    {
        var triggerCardAction = new TryTriggerCardEffectBattleAction(card, cardTarget, 1, isReflection);  
        
        pipeline.EnqueueFront(new NotifyCardExecutionCompletedBattleAction(card));
        pipeline.EnqueueFront(triggerCardAction);
        pipeline.Resume();
    }

    public void EndPlayerTurn()
    {
        scheduler.EndPlayerTurn();
    }

    private void OnCardEffectExecuted(CardEffectExecutedBattleEvent payload)
    {
        viewEventBus.Publish(new CardEffectExecuted(viewEventBus.GetNextSequenceId(), payload.ExecutedCard, payload.Caster, payload.Target));
    }
    private void OnEnemyActionExecuted(EnemyActionExecutedBattleEvent payload)
    {
        viewEventBus.Publish(new EnemyActionExecuted(viewEventBus.GetNextSequenceId(), payload.Actor, payload.Action));
    }
    private void OnBattleStatusEffectExecuted(BattleStatusEffectExecutedBattleEvent payload)
    {
        viewEventBus.Publish(new BattleStatusEffectExecuted(viewEventBus.GetNextSequenceId(), payload.Owner, payload.BattleStatusEffect));
    }
    private void OnCardCostChanged(CardCostChangedBattleEvent payload)
    {
        viewEventBus.Publish(new CardCostChanged(viewEventBus.GetNextSequenceId(), payload.Card, payload.CurrentCost));
    }
    private void OnCardReflectionChanged(CardReflectionChangedBattleEvent payload)
    {
        viewEventBus.Publish(new CardReflectionChanged(viewEventBus.GetNextSequenceId(), payload.Card, payload.IsReflection));
    }

    public BattleStatusEffectData GetStatusEffectData(string id)
    {
        return context.BattleStatusEffectDatabase.GetData(id);
    }
}