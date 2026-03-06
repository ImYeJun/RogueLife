using System;
using ViewEvent.GameRunView;
using ViewEvent.ScheduleSelecting;
using ViewEvent.ScheduleView;

public partial class GameRun
{
    private Random random;
    private int seed;
    private readonly Action onRunEnded;
    private int finishedSchedulesCount;

    private Player player;
    private BattleSystem battleSystem;
    private ScheduleSystem scheduleSystem;
    private RunDiarySystem runDiarySystem;

    private SpecialDiaryDatabase specialDiaryDatabase;
    private ScheduleDatabase scheduleDatabase;
    private EnemyDatabase enemyDatabase;
    private IncidentDatabase incidentDatabase;
    private TransactionChoiceDatabase transactionChoiceDatabase;
    private CardDatabase cardDatabase;
    private BelongingsDatabase belongingsDatabase;
    private BattleStatusEffectDatabase battleStatusEffectDatabase;

    private GameRunViewEventBus viewEventBus;

    public ISelectingScheduleViewCommander SelectingScheduleViewCommander { get => scheduleSystem;  }
    public ScheduleSelectingViewEventBus SelectingScheudleViewEventBus { get => scheduleSystem.SelectingScheduleViewEventBus; }

    public IScheduleViewCommander ScheduleViewCommander { get => scheduleSystem; }
    public ScheduleViewEventBus ScheduleViewEventBus { get => scheduleSystem.ScheduleViewEventBus; }
    public GameRunViewEventBus ViewEventBus { get => viewEventBus; }

    public GameRun(
        int seed, 
        (ScheduleSkeletonRule skeletonRule, ScheduleNodeTypeResolveRule typeResolveRule) rules,
        (BelongingsDatabase belongingsDatabase, CardDatabase cardDatabase, EnemyDatabase enemyDatabase, IncidentDatabase incidentDatabase, ScheduleDatabase scheduleDatabase, SpecialDiaryDatabase specialDiaryDatabase, TransactionChoiceDatabase transactionChoiceDatabase, BattleStatusEffectDatabase battleStatusEffectDatabase) databases,
        StartDeck startDeck,
        Action onRunEnded)
    {
        this.seed = seed;
        this.onRunEnded = onRunEnded;
        random = new Random(this.seed);

        specialDiaryDatabase = databases.specialDiaryDatabase;
        scheduleDatabase = databases.scheduleDatabase;
        enemyDatabase = databases.enemyDatabase;
        incidentDatabase = databases.incidentDatabase;
        transactionChoiceDatabase = databases.transactionChoiceDatabase;
        cardDatabase = databases.cardDatabase;
        belongingsDatabase = databases.belongingsDatabase;
        battleStatusEffectDatabase = databases.battleStatusEffectDatabase;

        player = new Player(startDeck, cardDatabase);
        runDiarySystem = new RunDiarySystem(databases.specialDiaryDatabase, databases.enemyDatabase, databases.incidentDatabase, databases.belongingsDatabase, databases.cardDatabase);
        battleSystem = new BattleSystem(random, databases.cardDatabase, databases.battleStatusEffectDatabase);
        scheduleSystem = new ScheduleSystem(random, rules.skeletonRule, rules.typeResolveRule, battleSystem, OnScheduleEnd, scheduleDatabase, transactionChoiceDatabase);

        viewEventBus = new GameRunViewEventBus();

        FieldContext fieldContext = new FieldContext(
            random : random,
            transactionChoiceDatabase : transactionChoiceDatabase,
            cardDatabase : cardDatabase,
            belongingsDatabase : belongingsDatabase,
            scheduleSystem : scheduleSystem,
            battleSystem : battleSystem,
            health : player.Health,
            actionCost : player.ActionCost,
            deck : player.Deck,
            belongingsBag : player.BelongingsBag
        );

        scheduleSystem.InitializeContext(fieldContext);
        player.BelongingsBag.InitializeContext(fieldContext);
    }

    public GameRun(
        (ScheduleSkeletonRule skeletonRule, ScheduleNodeTypeResolveRule typeResolveRule) rules,
        (BelongingsDatabase belongingsDatabase, CardDatabase cardDatabase, EnemyDatabase enemyDatabase, IncidentDatabase incidentDatabase, ScheduleDatabase scheduleDatabase, SpecialDiaryDatabase specialDiaryDatabase, TransactionChoiceDatabase transactionChoiceDatabase, BattleStatusEffectDatabase battleStatusEffectDatabase) databases,
        StartDeck startDeck,
        Action onRunEnded) 
        : this(new Random().Next(), rules, databases, startDeck, onRunEnded){}

    public void StartGame()
    {
        finishedSchedulesCount = 0;
    }

    public void OnScheduleDataUnsettled()
    {
        runDiarySystem.WriteDiary(player.Deck, player.BelongingsBag, false);
    }

    public void OnScheduleEnd(ScheduleHistory history)
    {
        finishedSchedulesCount++;
        runDiarySystem.RecordScheduleHistory(finishedSchedulesCount, history);
        
        if (history.HasEarlyExited)
        {
            //TODO 세이브 기능 만들기 (유저가 Esc 키 눌러서 나간 경우)
            return;
        }

        //TODO Diary 용도의 Player interface 만들기
        if (history.HasMentalBroken)
        {
            onRunEnded?.Invoke();
            viewEventBus.Publish(new RunEnded());
            // runDiarySystem.WriteDiary(player.Deck, player.BelongingsBag, false);
            return;
        }

        if (finishedSchedulesCount >= Constant.MAX_SCHEDULE_REPETITION)
        {
            onRunEnded?.Invoke();
            viewEventBus.Publish(new RunEnded());
            // runDiarySystem.WriteDiary(player.Deck, player.BelongingsBag, true);
        }
        else
        {
            viewEventBus.Publish(new ScheduleCleared());
        }
    }

    public void StartSchedule()
    {
        scheduleSystem.StartSchedule(
            currentStartCount : finishedSchedulesCount + 1,
            OnScheduleUnsettled : OnScheduleDataUnsettled
        );
    }
}