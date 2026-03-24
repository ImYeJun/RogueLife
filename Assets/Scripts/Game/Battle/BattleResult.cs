using Battle.BattleResultCommands;

public readonly struct BattleResult
{
    private readonly BattleResultCommand command;
    private readonly bool hasResolved;
    private readonly EnemyData mainEnemyData;

    public BattleResult(BattleResultCommand command, bool hasResolved, EnemyData mainEnemyData)
    {
        this.command = command;
        this.hasResolved = hasResolved;
        this.mainEnemyData = mainEnemyData;
    }

    public BattleResultCommand Command => command;
    public bool HasResolved => hasResolved;
    public EnemyData MainEnemyData => mainEnemyData;
}