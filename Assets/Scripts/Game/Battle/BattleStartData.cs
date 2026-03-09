using System.Collections.Generic;

public readonly struct BattleStartData
{
    public readonly int StartPhaseCount;
    public readonly int MaxActionCost;
    public readonly int FirstTurnDrawCount;
    public readonly int TurnStartDrawCount;
    public readonly List<Card> StartDrawDeck;
    public readonly BattlePlayer BattlePlayer;
    public readonly List<BattleEnemy> Enemies;

    public BattleStartData(int startPhaseCount, int maxActionCost, int firstTurnDrawCount, int turnStartDrawCount, List<Card> startDrawDeck, BattlePlayer battlePlayer, List<BattleEnemy> enemies)
    {
        StartPhaseCount = startPhaseCount;
        MaxActionCost = maxActionCost;
        FirstTurnDrawCount = firstTurnDrawCount;
        TurnStartDrawCount = turnStartDrawCount;
        StartDrawDeck = startDrawDeck;
        BattlePlayer = battlePlayer;
        Enemies = enemies;
    }
}