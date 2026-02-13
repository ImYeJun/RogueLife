using System.Collections.Generic;

public interface IRunDiaryEnemyDatabaseContext
{
    public List<EnemyData> AvailableEnemies { get; }
}