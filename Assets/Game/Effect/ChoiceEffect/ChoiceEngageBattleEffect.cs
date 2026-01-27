using System.Collections.Generic;

public class ChoiceEngageBattleEffect : IChoiceEffect
{
    private List<EnemyData> engaingEnemyData;
    private Player player;
    private int startPhaseCount;

    public ChoiceEngageBattleEffect(List<EnemyData> engaingEnemyData, Player player, int startPhaseCount)
    {
        this.engaingEnemyData = engaingEnemyData;
        this.player = player;
        this.startPhaseCount = startPhaseCount;
    }

    public void Execute(ChoiceContext context)
    {
        context.BattleSystem.EngageBattle(engaingEnemyData, startPhaseCount);
    }
}