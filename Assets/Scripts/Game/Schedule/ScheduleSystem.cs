using System;
using System.Collections.Generic;
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
    private Action onScheduleUnsettled;
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

    public void InitializeContext(FieldContext context)
    {
        this.context = context;

        context.Health.OnPlayerHurt += OnHealthHurt;
        context.Health.OnPlayerHealed += OnHealthHealed;
    }

    public void StartSchedule(int currentStartCount, Action OnScheduleUnsettled)
    {
        this.currentStartCount = currentStartCount;
        this.onScheduleUnsettled = OnScheduleUnsettled;

        var availableData = scheduleDatabase.AvailableScheduleData.OrderBy(data => random.Next()).Take(Constant.SELECTING_SCHEDULE_COUNT).ToList();
        scheduleSelectingViewEventBus.Publish(new ReadyToSelectSchedule(availableData, currentStartCount));
    }

    public void SettleCurrentScheduleData(ScheduleData data)
    {
        currentSchedule = scheduleGenerator.GenerateSchedule(random, data);

        currentSchedule.OnNodeEnter += OnEnterNode;
        currentSchedule.OnRequestNextNodeSelection += OnRequestNextNodeSelection;
        currentSchedule.OnRequestTransactionSelection += OnRequestTransactionSelection;
        currentSchedule.OnNodeExit += OnExitNode;
        currentSchedule.OnEnd += EndSchedule;
        
        scheduleSelectingViewEventBus.Publish(new ScheduleSettled());
    }

    public void SetBossData(EnemyData bossData)
    {
        if (currentSchedule == null) { throw new InvalidOperationException("[ScheduleSystem] Schedule is not settled."); }
        currentSchedule.SetBossData(bossData);
    }

    public void BroadcastCurrentState()
    {
        scheduleViewEventBus.Publish(new ScheduleStateSynced(
            schedule : currentSchedule,
            currentScheduleCount : currentStartCount,
            health : context.Health,
            actionCost : context.ActionCost,
            deck : context.Deck,
            belongingsBag : context.BelongingsBag
        ));
    }


    public void EnterStartNodeIfNeeded()
    {
        if (currentSchedule.HasStarted) { return; } 
        currentSchedule.EnterStartNode(context);
    }

    public void EndSchedule(ScheduleHistory history)
    {
        currentSchedule.OnNodeEnter -= OnEnterNode; 
        currentSchedule.OnRequestNextNodeSelection -= OnRequestNextNodeSelection;
        currentSchedule.OnRequestTransactionSelection -= OnRequestTransactionSelection;
        currentSchedule.OnNodeExit -= OnExitNode;
        currentSchedule.OnEnd -= EndSchedule;

        onScheduleEnd?.Invoke(history);
    }

    public void OnEnterNode(Node enteringNode)
    {
        scheduleViewEventBus.Publish(new NodeEntered(enteringNode));
    }
    public void OnExitNode(Node exitingNode)
    {
        scheduleViewEventBus.Publish(new NodeExited(exitingNode));
    }

    public void MoveCard(Card card, DeckType from, DeckType to)
    {
        if(context.Deck.TryMoveCard(card, from, to))
        {
            scheduleViewEventBus.Publish(new DeckChanged(context.Deck));
        }
    }
    public void MoveBelonings(Belongings belongings, BelongingsBagType from, BelongingsBagType to)
    {
        if(context.BelongingsBag.TryMoveBelongings(belongings, from, to))
        {
            scheduleViewEventBus.Publish(new BelongingsBagChanged(context.BelongingsBag));
        }
    }
    public void OnHealthHurt(PlayerHurt payload)
    {
        scheduleViewEventBus.Publish(payload);
    }
    public void OnHealthHealed(PlayerHealed payload)
    {
        scheduleViewEventBus.Publish(payload);
    }

    public void OnRequestNextNodeSelection(List<Node> nextNodes)
    {
        scheduleViewEventBus.Publish(new NextNodeSelectRequested(nextNodes));
    }
    public void SettleNextNode(Node nextNode)
    {
        currentSchedule.SettleNextNode(nextNode);
    }
    public void SettleTransactionChoice(TransactionChoiceOrder order)
    {
        currentSchedule.SettleTransactionChoice(order);
    }
    public void OnRequestTransactionSelection(Dictionary<TransactionChoiceOrder, TransactionChoiceData> choices)
    {
        scheduleViewEventBus.Publish(new TransactionSelectRequested(choices));
    }
}