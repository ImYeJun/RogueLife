using System.Collections.Generic;

public class BattleStartEvent : BattleEvent
{
    private int startPhaseCount;
    private int maxActionCost;
    private int fisrtTurnDrawCount;
    private int turnStartDrawCount;
    private List<Card> startDrawDeck;
    private BattlePlayer battlePlayer;
    private List<BattleEnemy> enemies;

    public BattleStartEvent(int startPhaseCount, int maxActionCost, int fisrtTurnDrawCount, int turnStartDrawCount, List<Card> startDrawDeck, BattlePlayer battlePlayer, List<BattleEnemy> enemies)
    {
        this.startPhaseCount = startPhaseCount;
        this.maxActionCost = maxActionCost;
        this.fisrtTurnDrawCount = fisrtTurnDrawCount;
        this.turnStartDrawCount = turnStartDrawCount;
        this.startDrawDeck = startDrawDeck;
        this.battlePlayer = battlePlayer;
        this.enemies = enemies;
    }

    public int StartPhaseCount { get => startPhaseCount; }
    public int MaxActionCost { get => maxActionCost; }
    public int FisrtTurnDrawCount { get => fisrtTurnDrawCount; }
    public int TurnStartDrawCount { get => turnStartDrawCount; }
    public List<Card> StartDrawDeck { get => startDrawDeck; }
    public BattlePlayer BattlePlayer { get => battlePlayer; }
    public List<BattleEnemy> Enemies { get => enemies; }
}