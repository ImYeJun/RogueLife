#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDeckHistory : IBattleDeckHistoryContext, IBattleEventObserveService
{
    private int phaseIndex;
    private Dictionary<int, List<CardPlayHistory>> usedHistory = new Dictionary<int, List<CardPlayHistory>>();
    //* <PhaseIndex, PlayedCardsDuringPlayerTurnWithSeuqnece>
    private Dictionary<int, List<Card>> gravedHistory = new Dictionary<int, List<Card>>();

    //* <PhaseIndex, GravedCardsDuringPlayerTurnWithSeuqnece>
    private List<CardPlayHistory> UsedCardsDuringBattle => usedHistory.Values.SelectMany(sel=>sel).ToList();
    private List<CardPlayHistory> CurrentPhaseUseHistory => usedHistory[phaseIndex];
    private List<Card> CurrentPhaseGravedHistory => gravedHistory[phaseIndex];
    
    public void RecordUseCard(Card card, bool isReflection = false)
    {
        usedHistory[phaseIndex].Add(new CardPlayHistory(card, isReflection));
    }
    public void RecordGravedCard(Card card)
    {
        gravedHistory[phaseIndex].Add(card);
    }
    
    public CardPlayHistory? GetRecentlyPlayedHistory(ICardBehaviourOwner? ignoringCardBehaviourOwner = null)
    {
        for (int i = usedHistory.Count - 1; i >= 0; i--)
        {
            var currentEra = usedHistory[i];

            for (int j = currentEra.Count - 1; j >= 0; j--)
            {
                var selectedHistory = currentEra[j];
                if (selectedHistory.UsedCard == ignoringCardBehaviourOwner) { continue; }
                return selectedHistory;
            }
        }
        return null;
    }
    public List<Card> GetRecentlyGravedCard(int amount)
    {
        var result = new List<Card>();

        int remainingAmount = amount;
        for (int i = gravedHistory.Count - 1; i >= 0; i--)
        {
            var currentEra = gravedHistory[i];

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
        switch (scope)
        {
            case BattleScope.PHASE:
                return usedHistory[phaseIndex].Any(history => 
                    (rarity == CardRarity.ANY || history.UsedCard.CurrentRarity == rarity) &&
                    (attribute == CardAttribute.ANY || history.UsedCard.CurrentAttribute == attribute) &&
                    (type == CardType.ANY || history.UsedCard.CurrentType == type)
                );
            case BattleScope.BATTLE:
                return UsedCardsDuringBattle.Any(history => 
                    (attribute == CardAttribute.ANY || history.UsedCard.CurrentAttribute == attribute) &&
                    (type == CardType.ANY || history.UsedCard.CurrentType == type)
                );
            default:
                throw new InvalidOperationException($"{scope} is not valid for checking played card");
        }
    }
    public bool HasPlayedCard(BattleScope scope)
    {
        return HasPlayedCard(CardRarity.ANY, CardAttribute.ANY, CardType.ANY, scope);
    }

    public int GetPlayedCardCount(BattleScope scope)
    {
        switch (scope)
        {
            case BattleScope.PHASE:
                return usedHistory[phaseIndex].Count;
            case BattleScope.BATTLE:
                return usedHistory.Values.Sum(list => list.Count);
            default:
                throw new InvalidOperationException($"{scope} is not valid for getting played card count");
        }
    }

    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        eventBus.Subscribe<BattleStartEvent>(InitiateHistory);
        eventBus.Subscribe<PhaseStartBattleEvent>(CreateNewEra);
    }
    public void InitiateHistory(BattleStartEvent payload)
    {
        usedHistory.Clear();
        gravedHistory.Clear();
        phaseIndex = -1;
    }
    public void CreateNewEra(PhaseStartBattleEvent payload)
    {
        phaseIndex++;
        usedHistory[phaseIndex] = new List<CardPlayHistory>();
        gravedHistory[phaseIndex] = new List<Card>();
    }
}

public struct CardPlayHistory
{
    private Card usedCard;
    private bool isReflection;

    public CardPlayHistory(Card usedCard, bool isReflection)
    {
        this.usedCard = usedCard;
        this.isReflection = isReflection;
    }

    public Card UsedCard { get => usedCard; }
    public bool IsReflection { get => isReflection; }
}