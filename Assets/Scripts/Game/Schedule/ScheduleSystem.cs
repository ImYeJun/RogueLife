using System;
using System.Linq;
using ViewEvent.ScheduleSelecting;
using ViewEvent.ScheduleView;

public class ScheduleSystem : IFieldScheduleSystem, ISelectingScheduleViewCommander, IScheduleViewCommander
{
    private System.Random random;
    private FieldContext context;
    private ScheduleDatabase scheduleDatabase;
    private ScheduleGenerator scheduleGenerator;
    private Action<ScheduleHistory> onScheduleEnd;
    private ScheduleSelectingViewEventBus scheduleSelectingViewEventBus;
    private ScheduleViewEventBus scheduleViewEventBus;

    private int currentStartCount;

    private Schedule currentSchedule;
    public Schedule CurrentSchedule { get => currentSchedule; }
    public ScheduleSelectingViewEventBus SelectingScheduleViewEventBus { get => scheduleSelectingViewEventBus; }
    public ScheduleViewEventBus ScheduleViewEventBus { get => scheduleViewEventBus; }

    public ScheduleSystem(
        System.Random random, ScheduleSkeletonRule skeletonRule, ScheduleNodeTypeResolveRule nodeTypeResolveRule, IEngageBattle battleSystem, Action<ScheduleHistory> onScheduleEnd,
        ScheduleDatabase scheduleDatabase, TransactionChoiceDatabase transactionChoiceDatabase
    )
    {
        this.random = random;
        this.onScheduleEnd = onScheduleEnd;
        this.scheduleDatabase = scheduleDatabase;

        scheduleSelectingViewEventBus = new ScheduleSelectingViewEventBus();
        scheduleViewEventBus = new ScheduleViewEventBus();
        scheduleGenerator = new ScheduleGenerator(skeletonRule, nodeTypeResolveRule, battleSystem);
    }

    public void StartSchedule(int currentStartCount, TransactionChoiceDatabase transactionChoiceDatabase, CardDatabase cardDatabase, BelongingsDatabase belongingsDatabase, BattleSystem battleSystem, Player player, Action OnScheduleUnsettled)
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
        
        this.currentStartCount = currentStartCount;
        var availableData = scheduleDatabase.AvailableScheduleData.OrderBy(data => random.Next()).Take(Constant.SELECING_SCHEUDLE_COUNT).ToList();
        scheduleSelectingViewEventBus.Publish(new ReadyToSelectSchedule(availableData, currentStartCount));
    }

    public void SettleCurrentScheduleData(ScheduleData data)
    {
        currentSchedule = scheduleGenerator.GenerateSchedule(random, data);
        currentSchedule.OnEnd += EndSchedule;
        currentSchedule.OnNodeMoved  += OnNodeMoved;


        scheduleSelectingViewEventBus.Publish(new ScheduleSettled());
    }

    public void BroadcastCurrentState()
    {
        scheduleViewEventBus.Publish(new ScheduleStateSynced(
            currentScheduleData : currentSchedule.Data,
            currentScheduleCount : currentStartCount,
            health : context.Health,
            actionCost : context.ActionCost,
            deck : context.Deck,
            belongingsBag : context.BelongingsBag
        ));
    }
    public void EnterStartNodeIfNeeded()
    {
        if (currentSchedule.HasStarted) { return; } //? Should this condtion checking be delegated to RootController?
        currentSchedule.EnterStartNode(context);
    }

    public void EndSchedule(ScheduleHistory history)
    {
        currentSchedule.OnNodeMoved -= OnNodeMoved; // 💡 짝맞춰서 해제 추가!
        currentSchedule.OnEnd -= EndSchedule;
        onScheduleEnd?.Invoke(history);
    }

    public void SetBossData(EnemyData bossData)
    {
        if (currentSchedule == null) { throw new InvalidOperationException("[ScheduleSystem] Schedule is not settled."); }
        currentSchedule.SetBossData(bossData);
    }

    public void OnNodeMoved(Node currentNode)
    {
        scheduleViewEventBus.Publish(new NodeMoved(currentNode));
    }
}
