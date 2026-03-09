using System.Collections.Generic;

public class BattleStartEvent : BattleEvent
{
    private int startPhaseCount;
    private int maxActionCost;
    private int firstTurnDrawCount;
    private int turnStartDrawCount;
    private List<Card> startDrawDeck;
    private BattlePlayer battlePlayer;
    private List<BattleBelongings> battleBelongings;
    private List<BattleEnemy> enemies;

    public BattleStartEvent(int startPhaseCount, int maxActionCost, int firstTurnDrawCount, int turnStartDrawCount, List<Card> startDrawDeck, BattlePlayer battlePlayer, List<BattleBelongings> battleBelongings, List<BattleEnemy> enemies)
    {
        this.startPhaseCount = startPhaseCount;
        this.maxActionCost = maxActionCost;
        this.firstTurnDrawCount = firstTurnDrawCount;
        this.turnStartDrawCount = turnStartDrawCount;
        this.startDrawDeck = startDrawDeck;
        this.battlePlayer = battlePlayer;
        this.battleBelongings = battleBelongings; // 추가됨
        this.enemies = enemies;
    }

    public int StartPhaseCount { get => startPhaseCount; }
    public int MaxActionCost { get => maxActionCost; }
    public int FirstTurnDrawCount { get => firstTurnDrawCount; } 
    public int TurnStartDrawCount { get => turnStartDrawCount; }
    public List<Card> StartDrawDeck { get => startDrawDeck; }
    public BattlePlayer BattlePlayer { get => battlePlayer; }
    public List<BattleBelongings> BattleBelongings { get => battleBelongings; } 
    public List<BattleEnemy> Enemies { get => enemies; }
}