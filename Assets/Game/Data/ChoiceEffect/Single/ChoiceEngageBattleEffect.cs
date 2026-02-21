using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Battle.Cards.Behaviours;
using UnityEditor.Graphs;
using UnityEngine;

[Serializable]
//! This feature is full of shit hacks!!!!! What a mess!!! Refactor it!!!!
public class ChoiceEngageBattleEffect : IChoiceEffect
{
    [SerializeField] private List<EnemyData> engaingEnemyData;
    private bool hasResolved;
    private int lossMentalityOnUnresolved;
    private EnemyResolveReward resolveReward;
    private FieldContext context;

    public ChoiceEngageBattleEffect() {}

    public void Execute(FieldContext context)
    {
        this.context = context;
        var mainEnemyData = engaingEnemyData.OrderByDescending(data => data.Tier).First();
        lossMentalityOnUnresolved = mainEnemyData.LossMentalityOnUnresolved;
        resolveReward = mainEnemyData.Reward;
        
        var slots = new List<EnemyDataSlot>();
        foreach (var data in engaingEnemyData)
        {
            slots.Add(new EnemyDataSlot(data));
        }

        context.HasEngagedBattleByChoiceEngageBattleEffect = true;
        context.BattleSystem.EngageBattle(context.Health, context.ActionCost, context.Deck, context.BelongingsBag, slots, OnBattleExit);
    }

    public void OnBattleExit(BattleResult result)
    {
        context.HasEngagedBattleByChoiceEngageBattleEffect = false;
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
                context.OnPlayerMentalBrokenForChoiceEngageBattleEffect.Invoke();
                return;
            case BattleResult.OUT_OF_MY_WAY:
                OutOfMyWay();
                return;
            default:
                throw new InvalidOperationException($"{result} is not expected to be used in battle result");
        }
        
        foreach (var enemyData in engaingEnemyData)
        {
            context.RecordEncounterEnemyForChoiceEngageBattleEffect.Invoke(enemyData, hasResolved);
        }
        
        context.RequestNextNodeSelectionForChoiceEngageBattleEffect.Invoke();
    }

    public void OutOfMyWay()
    {
        foreach (var enemyData in engaingEnemyData)
        {
            context.RecordEncounterEnemyForChoiceEngageBattleEffect.Invoke(enemyData, hasResolved);
        }

        
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
}