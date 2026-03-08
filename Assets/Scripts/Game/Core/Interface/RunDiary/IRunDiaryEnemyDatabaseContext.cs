using System.Collections.Generic;

public interface IRunDiaryEnemyDatabaseContext
{
    public List<EnemyEntity> AvailableEnemies { get; }
}