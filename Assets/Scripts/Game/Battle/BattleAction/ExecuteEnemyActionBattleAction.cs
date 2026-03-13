using Battle.Enemies.Actions;

public class ExecuteEnemyActionBattleAction : IBattleAction
{
    private IReadOnlyBattleEnemy actor;
    private EnemyAction action;

    public ExecuteEnemyActionBattleAction(IReadOnlyBattleEnemy actor, EnemyAction action)
    {
        this.actor = actor;
        this.action = action;
    }

    public EnemyAction Action { get => action;  }

    public void Execute(BattleContext context)
    {
        context.EventBus.Publish(new EnemyActionExecutedBattleEvent(actor, action));
        action.Execute(context);
    }
}