using System.Collections.Generic;

public class BattleStartEvent : BattleEvent
{
    private readonly int startPhaseCount;
    private readonly int maxActionCost;
    private readonly int firstTurnDrawCount;
    private readonly int turnStartDrawCount;
    private readonly List<Card> startDrawDeck;
    private readonly BattlePlayer battlePlayer;
    private readonly List<BattleBelongings> battleBelongings;
    private readonly List<BattleEnemy> enemies;
    private readonly EnemyData mainEnemyData;

    public BattleStartEvent(int startPhaseCount, int maxActionCost, int firstTurnDrawCount, int turnStartDrawCount, List<Card> startDrawDeck, BattlePlayer battlePlayer, List<BattleBelongings> battleBelongings, List<BattleEnemy> enemies, EnemyData mainEnemyData)
    {
        this.startPhaseCount = startPhaseCount;
        this.maxActionCost = maxActionCost;
        this.firstTurnDrawCount = firstTurnDrawCount;
        this.turnStartDrawCount = turnStartDrawCount;
        this.startDrawDeck = startDrawDeck;
        this.battlePlayer = battlePlayer;
        this.battleBelongings = battleBelongings;
        this.enemies = enemies;
        this.mainEnemyData = mainEnemyData;
    }

    public int StartPhaseCount => startPhaseCount;
    public int MaxActionCost => maxActionCost;
    public int FirstTurnDrawCount => firstTurnDrawCount; 
    public int TurnStartDrawCount => turnStartDrawCount;
    public List<Card> StartDrawDeck => startDrawDeck;
    public BattlePlayer BattlePlayer => battlePlayer;
    public List<BattleBelongings> BattleBelongings => battleBelongings; 
    public List<BattleEnemy> Enemies => enemies;
    public EnemyData MainEnemyData => mainEnemyData;
}