public abstract class EnemyAction
{
    private IEnemyBehaviourOwner owner;
    public abstract void Execute(BattleContext context);
}