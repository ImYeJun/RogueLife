using System.Collections.Generic;
using Battle.HurtSources;

public interface IEnemyBehaviourOwner
{
    public BattleEntity AsEntity { get; }
    public BattleHurtSource AsHurtSource { get; }
    public EnemyData Data { get; }
    public int PreviousActionCount { get; }
    public bool IsFirstAction { get; }
}