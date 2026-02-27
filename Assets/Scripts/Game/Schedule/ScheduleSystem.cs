using System;
using System.Linq;
using ViewEvent.ScheduleSelecting;

public class ScheduleSystem : IFieldScheduleSystem, ISelectingScheduleViewCommander
{
    private System.Random random;
    private FieldContext context;
    private ScheduleDatabase scheduleDatabase;
    private ScheduleGenerator scheduleGenerator;
    private Action<ScheduleHistory> onScheduleEnd;
    private ScheduleSelectingViewEventBus viewEventBus;
    private ScheduleSelectingViewEventBus scheduleSelectingViewEventBus;

    private Schedule currentSchedule;
    public Schedule CurrentSchedule { get => currentSchedule; }
    public ScheduleSelectingViewEventBus SelectingScheduleViewEventBus { get => viewEventBus; }

    public ScheduleSystem(
        System.Random random, ScheduleSkeletonRule skeletonRule, ScheduleNodeTypeResolveRule nodeTypeResolveRule, IEngageBattle battleSystem, Action<ScheduleHistory> onScheduleEnd,
        ScheduleDatabase scheduleDatabase, TransactionChoiceDatabase transactionChoiceDatabase
    )
    {
        this.random = random;
        this.onScheduleEnd = onScheduleEnd;
        this.scheduleDatabase = scheduleDatabase;

        viewEventBus = new ScheduleSelectingViewEventBus();
        scheduleGenerator = new ScheduleGenerator(skeletonRule, nodeTypeResolveRule, battleSystem);
    }

    public void StartSchdule(int currentStartCount, TransactionChoiceDatabase transactionChoiceDatabase, CardDatabase cardDatabase, BelongingsDatabase belongingsDatabase, BattleSystem battleSystem, Player player, Action OnScheduleUnsettled)
    {
        context = new FieldContext(
            random : random,
            transactionChoiceDatabase : transactionChoiceDatabase,
            cardDatabase : cardDatabase,
            belongingsDatabase : belongingsDatabase,
            scheduleSystem : this,
            battleSystem : battleSystem,
            health : player.Health,
            actionCost : player.ActionCost,
            deck : player.Deck,
            belongingsBag : player.BelongingsBag
        );
        
        var availiableData = scheduleDatabase.AvailableScheduleData.OrderBy(data => random.Next()).Take(Constant.SELECING_SCHEUDLE_COUNT).ToList();
        // viewEventBus.Publish(new ReadToSelectSchedule(availiableData, currentStartCount));
    }

    public void SettleCurrentScheduleData(ScheduleData data)
    {
        currentSchedule = scheduleGenerator.GenerateSchedule(random, data);
        currentSchedule.OnEnd += EndSchedule;

        currentSchedule.EnterStartNode(context);
    }

    public void EndSchedule(ScheduleHistory history)
    {
        currentSchedule.OnEnd -= EndSchedule;
        onScheduleEnd?.Invoke(history);
    }

    public void SetBossData(EnemyData bossData)
    {
        if (currentSchedule == null) { throw new InvalidOperationException("[ScheduleSystem] Schedule is not settled."); }
        currentSchedule.SetBossData(bossData);
    }
}
