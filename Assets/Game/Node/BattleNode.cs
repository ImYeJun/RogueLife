using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;

public class BattleNode : Node
{
    private IEngageBattle battleSystem;
    private List<EnemyDataSlot> engagingEnemiesDataSlot;
    private bool hasResolved;
    private int lossMentalityOnUnresolved;
    private EnemyResolveReward resolveReward;

    public BattleNode(Guid skeletonId, Action<Node, FieldContext> OnMoveRequest, IEngageBattle battleSystem, List<EnemyDataSlot> engagingEnemiesDataSlot) : base(OnMoveRequest, skeletonId)
    {
        this.battleSystem = battleSystem;
        this.engagingEnemiesDataSlot = engagingEnemiesDataSlot;

        var mainEnemyData = engagingEnemiesDataSlot.OrderByDescending(slot => slot.Data.Tier).First().Data;
        lossMentalityOnUnresolved = mainEnemyData.LossMentalityOnUnresolved;
        resolveReward = mainEnemyData.Reward;
    }
    public override void OnEnter(FieldContext context, ScheduleHistory scheduleHistory)
    {
        //TODO : engagingEnemiesData에 따라 적 일상 UI 띄우기
        base.OnEnter(context, scheduleHistory);

        //TODO : engagingEnemiesData에 따라 encounterLine 연출 띄우기

        battleSystem.EngageBattle(context.Health, context.ActionCost, context.Deck, context.BelongingsBag, engagingEnemiesDataSlot, OnBattleExit);
    }

    public void OnBattleExit(BattleResult result)
    {
        context.Health.OnMentalBreakDown += OnPlayerMentalBroken;
        hasResolved = result == BattleResult.PLAYER_WIN;

        switch (result)
        {
            case BattleResult.PLAYER_WIN:
                GetReward();
                break;
            case BattleResult.ALL_PHASE_END:
                GetPenalty();
                break;
            case BattleResult.PLAYER_DIED:
                OnPlayerMentalBroken();
                return;
            case BattleResult.OUT_OF_MY_WAY:
                OnOutOfMyWay();
                return;
            default:
                throw new InvalidOperationException($"{result} is not expected to be used in battle result");
        }
        
        RequestNextNodeSelection();
    }

    private void OnOutOfMyWay()
    {
        var randomNextNode = nextNodes[context.Random.Next(nextNodes.Count)];
        
        OnExit(randomNextNode);
    }

    private void GetReward()
    {
        if (resolveReward is CardEnemyResolveReward cardReward)
        {
            var rewardCards = context.CardDatabase.GetEnemyResolveReward(context.Random, cardReward);
            foreach (var card in rewardCards)
            {
                if (context.Deck.TryObtainCard(card))
                {
                    //TODO 카드 획득 연출 띄우기
                }
                else
                {
                    //TODO 획득 실패 연출 띄우기
                }
            }
        }
    }

    private void GetPenalty()
    {
        context.Health.HurtBattleHealth(lossMentalityOnUnresolved, true);
    }

    protected override void OnExit(Node nextNode)
    {
        foreach (var enemyDataSlot in engagingEnemiesDataSlot)
        {
            var enemyData = enemyDataSlot.Data;

            if (enemyData.Tier == EnemyTier.BOSS) { scheduleHistory.RecordEncounterBoss(enemyData, hasResolved); }
            else { scheduleHistory.RecordEncounterEnemy(enemyData, hasResolved); }
        }
        RecordBelongingsEquipping();

        context.Health.OnMentalBreakDown -= OnPlayerMentalBroken;
        base.OnExit(nextNode);
    }
}