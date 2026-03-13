using Battle.Enemies.Actions;

public class EnemyActionExecutedBattleEvent : BattleEvent{
    private readonly IReadOnlyBattleEnemy actor;
    private readonly EnemyAction action;

    public EnemyActionExecutedBattleEvent(IReadOnlyBattleEnemy actor, EnemyAction action)
    {
        this.actor = actor;
        this.action = action;
    }

    public IReadOnlyBattleEnemy Actor => actor;
    public EnemyAction Action => action;
}