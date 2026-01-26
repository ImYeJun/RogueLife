using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleSystem
{
    private int remainPhaseCount;
    private bool isBattleEnd;

    private BattleContext currentBattleContext;

    private List<BattleEnemy> battleEnemies = new List<BattleEnemy>();
    private BattlePlayer battlePlayer;
    private PlayerActionCost playerActionCost;

    private List<Card> deckZoneCards = new List<Card>();
    private List<Card> handZoneCards = new List<Card>();
    private List<Card> graveZoneCards = new List<Card>();

    private bool isFirstPlayerTurn;
    private int startTurnDrawCount = Constant.BASE_START_TURN_DRAW_COUNT;
    private int fisrtTurnDrawCount = Constant.BASE_FIRST_TURN_DRAW_COUNT;
    private int maxHandZoneCardCount = Constant.BASE_MAX_HAND_ZONE_CARD_COUNT;

    public event Action<BattleResult> OnBattleExit;

    private void UpdateBattleContext()
    {
        currentBattleContext = new BattleContext();
    }

    public void EngageBattle(List<EnemyData> enemiesData, Player player, int startPhaseCount)
    {
        battleEnemies.Clear();
        deckZoneCards.Clear();
        handZoneCards.Clear();
        graveZoneCards.Clear();
        isBattleEnd = false;
        isFirstPlayerTurn = true;

        UpdateBattleContext();

        //TODO : 전투 진입 연출 구현

        foreach (EnemyData enemyData in enemiesData)
        {
            battleEnemies.Add(new BattleEnemy(enemyData));
        }

        PlayerHealth playerHealth = player.Health;
        battlePlayer = new BattlePlayer(playerHealth);
        playerHealth.OnMentalBreakDown += OnPlayerMentalBreakDown;
        playerActionCost = player.ActionCost;
        deckZoneCards = player.Deck.MainDeckCards.Select(card => new Card(card)).ToList();

        remainPhaseCount = startPhaseCount;

        StartPhase();
    }

    private void StartPhase()
    {
        //TODO : 페이즈 시작 연출 구현
        foreach (BattleEnemy enemy in battleEnemies)
        {
            enemy.PlanNextAction();
        }

        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        //TODO : 플레이어 턴 시작 연출 실행

        playerActionCost.Refill();

        DrawCard();
        isFirstPlayerTurn = false;
    }

    private void DrawCard()
    {
        int drawCardCount = isFirstPlayerTurn ? fisrtTurnDrawCount : startTurnDrawCount;

        if (deckZoneCards.Count() < drawCardCount) { RefillDeckFromGrave(); }

        for (int i = 0; i < drawCardCount; i++)
        {
            if (deckZoneCards.Count() == 0) { break; }

            int randomIndex = UnityEngine.Random.Range(0, deckZoneCards.Count());

            Card drawCard = deckZoneCards[randomIndex];
            deckZoneCards.RemoveAt(randomIndex);
            handZoneCards.Add(drawCard);

            if (handZoneCards.Count() > maxHandZoneCardCount)
            {
                handZoneCards.Remove(drawCard);
                graveZoneCards.Add(drawCard);
            }
        }
    }

    private void RefillDeckFromGrave()
    {
        if (graveZoneCards.Count == 0) return;

        foreach (Card card in graveZoneCards)
        {
            card.ApplyReflection();
            deckZoneCards.Add(card);
        }

        graveZoneCards.Clear();
        deckZoneCards = deckZoneCards.OrderBy(_ => Guid.NewGuid()).ToList();
    }

    public bool TryExecuteCard(Card card)
    {
        if (!handZoneCards.Contains(card))
        {
            Debug.Log("DeckZone doesn't have the given card.");
            return false;
        }
        if (!playerActionCost.TrySpend(card.CurrentActionCost)) { return false; }

        UpdateBattleContext();
        card.Execute(currentBattleContext);
        handZoneCards.Remove(card);

        if (card.IsReflectionApplied)
        {
            card.UnapplyReflection();
            deckZoneCards.Add(card);
        }
        else
        {
            graveZoneCards.Add(card);
        }

        if (HasPlayerWin()) { OnPlayerWin(); }
        return true;
    }

    private bool HasPlayerWin() { return currentBattleContext.HasSpecialVictoryExecuted || battleEnemies.All(enemy => enemy.IsDead); }

    private void EndPlayerTurn()
    {
        //TODO : 플레이어 턴 종료 연출 구현하기
        StartEnemyTurn();
    }

    private void StartEnemyTurn()
    {
        //TODO : 적턴 시작 연출 구현하기
        ExecuteEnemiesAction();
        EndEnemyTurn();
    }

    private void ExecuteEnemiesAction()
    {
        foreach (BattleEnemy enemy in battleEnemies)
        {
            if (isBattleEnd) { return; }
            if (enemy.IsDead) { continue; }
            
            foreach (BattleAction action in enemy.PlannedActions)
            {
                UpdateBattleContext();
                action.Execute(currentBattleContext);

                if (isBattleEnd) { return; }
            }
        }
    }

    private void EndEnemyTurn()
    {
        //TODO : 적 턴 종료 연출 실행
        EndPhase();
    }

    private void EndPhase()
    {
        if (--remainPhaseCount <= 0) { OnAllPhaseEnded(); }
        else { StartPhase(); }
    }

    private void OnAllPhaseEnded()
    {
        OnBattleEnd();
        //TODO : 페이즈 소모로 인한 패널티 받기 로직 구현하기
        ExitBattle(BattleResult.ALL_PHASE_END);
    }

    private void OnPlayerWin()
    {
        OnBattleEnd();
        //TODO : 전투 승리 선택 UI 띄우기
        ExitBattle(BattleResult.PLAYER_WIN);
    }

    public void OnPlayerMentalBreakDown()
    {
        OnBattleEnd();
        //TODO : 전투 플레이어 사망 연출 구현하기
        ExitBattle(BattleResult.PLAYER_DIED);
    }
    
    private void OnBattleEnd()
    {
        isBattleEnd = true;
        currentBattleContext?.RequestBattleEnd();
    }

    public void ExitBattle(BattleResult result)
    {
        //TODO: 전투 종료 연출 실행
        battlePlayer.Health.OnMentalBreakDown -= OnPlayerMentalBreakDown;
        OnBattleExit?.Invoke(result);
    }
}