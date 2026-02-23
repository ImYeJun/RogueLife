using Battle.Enemies.Actions;

public class ExecuteEnemyActionBattleAction : IBattleAction
{
    private EnemyAction action;

    public ExecuteEnemyActionBattleAction(EnemyAction action)
    {
        this.action = action;
    }

    public EnemyAction Action { get => action;  }

    public void Execute(BattleContext context)
    {
        action.Execute(context);
    }
}