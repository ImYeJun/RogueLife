#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDeckHistory : IBattleDeckHistoryContext, IBattleEventObserveService
{
    private int phaseIndex = -1;
    private int playerTurnIndex = -1;

    private Dictionary<int, List<ExecuteCardEffectHistory>> usedHistoryByPhase = new Dictionary<int, List<ExecuteCardEffectHistory>>();
    private Dictionary<int, List<ExecuteCardEffectHistory>> executeEffectHistoryByPhase = new Dictionary<int, List<ExecuteCardEffectHistory>>();
    private Dictionary<int, List<Card>> gravedHistoryByPhase = new Dictionary<int, List<Card>>();

    private Dictionary<int, List<ExecuteCardEffectHistory>> usedHistoryByTurn = new Dictionary<int, List<ExecuteCardEffectHistory>>();
    private Dictionary<int, List<ExecuteCardEffectHistory>> executeEffectHistoryByTurn = new Dictionary<int, List<ExecuteCardEffectHistory>>();
    private Dictionary<int, List<Card>> gravedHistoryByTurn = new Dictionary<int, List<Card>>();

    private List<ExecuteCardEffectHistory> UsedCardsDuringBattle => usedHistoryByPhase.Values.SelectMany(sel => sel).ToList();
    
    public void RecordExecuteCardEffect(Card card, bool isReflection = false)
    {
        var history = new ExecuteCardEffectHistory(card, isReflection);
        
        if (executeEffectHistoryByPhase.ContainsKey(phaseIndex))
            executeEffectHistoryByPhase[phaseIndex].Add(history);
            
        if (executeEffectHistoryByTurn.ContainsKey(playerTurnIndex))
            executeEffectHistoryByTurn[playerTurnIndex].Add(history);
    }

    public void RecordUseCard(Card card, bool isReflection = false)
    {
        var history = new ExecuteCardEffectHistory(card, isReflection);
        
        if (usedHistoryByPhase.ContainsKey(phaseIndex))
            usedHistoryByPhase[phaseIndex].Add(history);
            
        if (usedHistoryByTurn.ContainsKey(playerTurnIndex))
            usedHistoryByTurn[playerTurnIndex].Add(history);
    }

    public void RecordGravedCard(Card card)
    {
        if (gravedHistoryByPhase.ContainsKey(phaseIndex))
            gravedHistoryByPhase[phaseIndex].Add(card);
            
        if (gravedHistoryByTurn.ContainsKey(playerTurnIndex))
            gravedHistoryByTurn[playerTurnIndex].Add(card);
    }

    public ExecuteCardEffectHistory? GetRecentlyPlayedHistory(ICardBehaviourOwner? ignoringCardBehaviourOwner = null)
    {
        for (int i = usedHistoryByPhase.Count - 1; i >= 0; i--)
        {
            var currentEra = usedHistoryByPhase[i];
            for (int j = currentEra.Count - 1; j >= 0; j--)
            {
                var selectedHistory = currentEra[j];
                if (selectedHistory.UsedCard == ignoringCardBehaviourOwner) { continue; }
                return selectedHistory;
            }
        }
        return null;
    }

    public List<ExecuteCardEffectHistory>? GetRecentPhasePlayedHistory(ICardBehaviourOwner? ignoringCardBehaviourOwner = null)
    {
        if (!usedHistoryByPhase.ContainsKey(phaseIndex)) { return null; }

        var recentPhaseHistory = usedHistoryByPhase[phaseIndex];
        return ignoringCardBehaviourOwner is null ? recentPhaseHistory 
            : recentPhaseHistory.Where(history => history.UsedCard != ignoringCardBehaviourOwner).ToList();
    }

    public List<Card> GetRecentlyGravedCard(int amount)
    {
        var result = new List<Card>();
        int remainingAmount = amount;

        for (int i = gravedHistoryByPhase.Count - 1; i >= 0; i--)
        {
            var currentEra = gravedHistoryByPhase[i];
            for (int j = currentEra.Count - 1; j >= 0; j--)
            {
                result.Add(currentEra[j]);
                if (--remainingAmount <= 0) { return result; } 
            }
        }

        return result;
    }

    public bool HasPlayedCard(CardRarity rarity, CardAttribute attribute, CardType type, BattleScope scope)
    {
        var targetList = GetUsedHistoryByScope(scope);
        if (targetList == null || targetList.Count == 0) return false;

        return targetList.Any(history => 
            (rarity == CardRarity.ANY || history.UsedCard.CurrentRarity == rarity) &&
            (attribute == CardAttribute.ANY || history.UsedCard.CurrentAttribute == attribute) &&
            (type == CardType.ANY || history.UsedCard.CurrentType == type)
        );
    }

    public bool HasPlayedCard(BattleScope scope)
    {
        return HasPlayedCard(CardRarity.ANY, CardAttribute.ANY, CardType.ANY, scope);
    }

    public int GetPlayedCardCount(BattleScope scope)
    {
        var targetList = GetUsedHistoryByScope(scope);
        return targetList?.Count ?? 0;
    }

    public int GetExecuteCardEffectCount(BattleScope scope)
    {
        var targetList = GetExecuteHistoryByScope(scope);
        return targetList?.Count ?? 0;
    }

    private List<ExecuteCardEffectHistory>? GetUsedHistoryByScope(BattleScope scope)
    {
        return scope switch
        {
            BattleScope.TURN => usedHistoryByTurn.ContainsKey(playerTurnIndex) ? usedHistoryByTurn[playerTurnIndex] : new List<ExecuteCardEffectHistory>(),
            BattleScope.PHASE => usedHistoryByPhase.ContainsKey(phaseIndex) ? usedHistoryByPhase[phaseIndex] : new List<ExecuteCardEffectHistory>(),
            BattleScope.BATTLE => UsedCardsDuringBattle,
            _ => throw new InvalidOperationException($"[BattleDeckHistory] {scope} is not valid for getting played card history")
        };
    }

    private List<ExecuteCardEffectHistory>? GetExecuteHistoryByScope(BattleScope scope)
    {
        return scope switch
        {
            BattleScope.TURN => executeEffectHistoryByTurn.ContainsKey(playerTurnIndex) ? executeEffectHistoryByTurn[playerTurnIndex] : new List<ExecuteCardEffectHistory>(),
            BattleScope.PHASE => executeEffectHistoryByPhase.ContainsKey(phaseIndex) ? executeEffectHistoryByPhase[phaseIndex] : new List<ExecuteCardEffectHistory>(),
            BattleScope.BATTLE => executeEffectHistoryByPhase.Values.SelectMany(sel => sel).ToList(),
            _ => throw new InvalidOperationException($"[BattleDeckHistory] {scope} is not valid for getting execute card effect count")
        };
    }

    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        eventBus.Subscribe<BattleStartEvent>(InitiateHistory);
        eventBus.Subscribe<PhaseStartBattleEvent>(CreateNewPhaseEra);
        eventBus.Subscribe<PlayerTurnStartBattleEvent>(CreateNewTurnEra);
    }

    public void InitiateHistory(BattleStartEvent payload)
    {
        executeEffectHistoryByPhase.Clear();
        usedHistoryByPhase.Clear();
        gravedHistoryByPhase.Clear();

        executeEffectHistoryByTurn.Clear();
        usedHistoryByTurn.Clear();
        gravedHistoryByTurn.Clear();

        phaseIndex = -1;
        playerTurnIndex = -1;
    }

    public void CreateNewPhaseEra(PhaseStartBattleEvent payload)
    {
        phaseIndex++;
        executeEffectHistoryByPhase[phaseIndex] = new List<ExecuteCardEffectHistory>();
        usedHistoryByPhase[phaseIndex] = new List<ExecuteCardEffectHistory>();
        gravedHistoryByPhase[phaseIndex] = new List<Card>();
    }

    public void CreateNewTurnEra(PlayerTurnStartBattleEvent payload)
    {
        playerTurnIndex++;
        executeEffectHistoryByTurn[playerTurnIndex] = new List<ExecuteCardEffectHistory>();
        usedHistoryByTurn[playerTurnIndex] = new List<ExecuteCardEffectHistory>();
        gravedHistoryByTurn[playerTurnIndex] = new List<Card>();
    }
}

public struct ExecuteCardEffectHistory
{
    private Card usedCard;
    private bool isReflection;

    public ExecuteCardEffectHistory(Card usedCard, bool isReflection)
    {
        this.usedCard = usedCard;
        this.isReflection = isReflection;
    }

    public Card UsedCard { get => usedCard; }
    public bool IsReflection { get => isReflection; }
}