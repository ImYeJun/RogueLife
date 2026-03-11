using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ViewEvent.ScheduleSelecting;
using ViewEvent.ScheduleView;

public class ScheduleSystem : IFieldScheduleSystem, ISelectingScheduleViewCommander, IScheduleViewCommander
{
    private SequenceIdGenerator sequenceIdGenerator;

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
        
        sequenceIdGenerator = new SequenceIdGenerator();
        scheduleSelectingViewEventBus = new ScheduleSelectingViewEventBus();
        scheduleViewEventBus = new ScheduleViewEventBus();
        scheduleGenerator = new ScheduleGenerator(skeletonRule, nodeTypeResolveRule, battleSystem);
    }

    public void InitializeContext(FieldContext context)
    {
        this.context = context;

        context.Health.OnHurt += OnHealthHurt;
        context.Health.OnHealed += OnHealthHealed;
    }

    public void StartSchedule(int currentStartCount, Action OnScheduleUnsettled)
    {
        this.currentStartCount = currentStartCount;
        this.onScheduleUnsettled = OnScheduleUnsettled;

        var availableData = scheduleDatabase.AvailableScheduleData.OrderBy(data => random.Next()).Take(Constant.SELECTING_SCHEDULE_COUNT).ToList();
        scheduleSelectingViewEventBus.Publish(new ReadyToSelectSchedule(sequenceIdGenerator.GetNextId(), availableData, currentStartCount));
    }

    public void SettleCurrentScheduleData(ScheduleData data, Vector2 selectPos)
    {
        currentSchedule = scheduleGenerator.GenerateSchedule(random, data);

        currentSchedule.OnNodeEnter += OnEnterNode;
        currentSchedule.OnRequestNextNodeSelection += OnRequestNextNodeSelection;
        currentSchedule.OnRequestIncidentSelection += OnRequestIncidentSelection;
        currentSchedule.OnRequestTransactionSelection += OnRequestTransactionSelection;
        currentSchedule.OnRequestBattleTransition += OnRequestBattleTransition;
        currentSchedule.OnNodeExit += OnExitNode;
        currentSchedule.OnEnd += EndSchedule;
        
        scheduleSelectingViewEventBus.Publish(new ScheduleSettled(sequenceIdGenerator.GetNextId(), selectPos));
    }

    public void SetBossData(EnemyEntity bossEntity)
    {
        if (currentSchedule == null) { throw new InvalidOperationException("[ScheduleSystem] Schedule is not settled."); }
        currentSchedule.SetBossData(bossEntity);
    }

    public void BroadcastCurrentState()
    {
        scheduleViewEventBus.Publish(new ScheduleStateSynced(
            sequenceId : sequenceIdGenerator.GetNextId(), 
            schedule : currentSchedule,
            currentScheduleCount : currentStartCount,
            health : context.Health,
            actionCost : context.ActionCost,
            deck : context.Deck,
            belongingsBag : context.BelongingsBag
        ));
    }

    public void ResumeSchedule()
    {
        if (currentSchedule == null)
        {
            throw new InvalidOperationException("[ScheduleSystem] Current schedule is not settled yet.");
        }

        if (!currentSchedule.HasStarted)
        {
            currentSchedule.EnterStartNode(context);
            return;
        }

        if (currentSchedule.HasPendingBattleResult())
        {
            scheduleViewEventBus.Publish(new ReturnedFromBattle(sequenceIdGenerator.GetNextId()));
            currentSchedule.ResolvePendingResult();
            return;
        }


    }

    public void EndSchedule(ScheduleHistory history)
    {
        currentSchedule.OnNodeEnter -= OnEnterNode; 
        currentSchedule.OnRequestNextNodeSelection -= OnRequestNextNodeSelection;
        currentSchedule.OnRequestIncidentSelection -= OnRequestIncidentSelection;
        currentSchedule.OnRequestTransactionSelection -= OnRequestTransactionSelection;
        currentSchedule.OnRequestBattleTransition -= OnRequestBattleTransition;
        currentSchedule.OnNodeExit -= OnExitNode;
        currentSchedule.OnEnd -= EndSchedule;

        onScheduleEnd?.Invoke(history);
    }

    public void OnEnterNode(Node enteringNode)
    {
        scheduleViewEventBus.Publish(new NodeEntered(sequenceIdGenerator.GetNextId(), enteringNode));
    }
    public void OnExitNode(Node exitingNode)
    {
        scheduleViewEventBus.Publish(new NodeExited(sequenceIdGenerator.GetNextId(), exitingNode));
    }

    public void MoveCard(Card card, DeckType from, DeckType to)
    {
        if(context.Deck.TryMoveCard(card, from, to))
        {
            scheduleViewEventBus.Publish(new DeckChanged(sequenceIdGenerator.GetNextId(), context.Deck));
        }
    }
    public void MoveBelonings(Belongings belongings, BelongingsBagType from, BelongingsBagType to)
    {
        if(context.BelongingsBag.TryMoveBelongings(belongings, from, to))
        {
            scheduleViewEventBus.Publish(new BelongingsBagChanged(sequenceIdGenerator.GetNextId(), context.BelongingsBag));
        }
    }

    private void OnHealthHurt(int actualDamage, int actualMentalityDamage, bool isOverflowed)
    {
        scheduleViewEventBus.Publish(new PlayerHurt(sequenceIdGenerator.GetNextId(), context.Health, actualDamage, actualMentalityDamage, isOverflowed));
    }
    private void OnHealthHealed(bool isOverflowed, int actualBattleHealthHeal, int actualMentalityHeal)
    {
        scheduleViewEventBus.Publish(new PlayerHealed(sequenceIdGenerator.GetNextId(), context.Health, isOverflowed, actualBattleHealthHeal, actualMentalityHeal));
    }

    public void OnRequestNextNodeSelection(List<Node> nextNodes)
    {
        scheduleViewEventBus.Publish(new NextNodeSelectRequested(sequenceIdGenerator.GetNextId(), nextNodes));
    }
    public void SettleNextNode(Node nextNode)
    {
        currentSchedule.SettleNextNode(nextNode);
    }
    public void SettleTransactionChoice(TransactionChoiceOrder order)
    {
        currentSchedule.SettleTransactionChoice(order);
    }
    public void SettleIncidentChoice(DeterminedIncidentChoice choice)
    {
        currentSchedule.SettleIncidentChoice(choice);
    }

    public void OnRequestTransactionSelection(Dictionary<TransactionChoiceOrder, TransactionChoiceData> choices)
    {
        scheduleViewEventBus.Publish(new TransactionSelectRequested(sequenceIdGenerator.GetNextId(), choices));
    }
    public void OnRequestIncidentSelection(List<DeterminedIncidentChoice> choices)
    {
        scheduleViewEventBus.Publish(new IncidentSelectRequested(sequenceIdGenerator.GetNextId(), choices));
    }

    public void OnRequestBattleTransition()
    {
        scheduleViewEventBus.Publish(new BattleEngaged(sequenceIdGenerator.GetNextId()));
    }
}