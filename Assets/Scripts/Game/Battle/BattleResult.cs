using Battle.BattleResultCommands;

public readonly struct BattleResult
{
    private readonly BattleResultCommand command;
    private readonly bool hasResolved;
    private readonly EnemyData mainEnemyData;
    private readonly bool isNextNodeSelectable;

    public BattleResult(BattleResultCommand command, bool hasResolved, EnemyData mainEnemyData, bool isNextNodeSelectable)
    {
        this.command = command;
        this.hasResolved = hasResolved;
        this.mainEnemyData = mainEnemyData;
        this.isNextNodeSelectable = isNextNodeSelectable;
    }

    public BattleResultCommand Command => command;
    public bool HasResolved => hasResolved;
    public EnemyData MainEnemyData => mainEnemyData;
    public bool IsNextNodeSelectable => isNextNodeSelectable;
}