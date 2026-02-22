using Battle.HurtSources;

public class HurtEnemyBattleAction : IBattleAction, IEntityTargetedBattleAction
{
    private BattleEnemy enemy;
    private BattleHurtSource source;
    private int amount;

    public HurtEnemyBattleAction(BattleEnemy enemy, BattleHurtSource source, int amount)
    {
        this.enemy = enemy;
        this.source = source;
        this.amount = amount;
    }

    public BattleEnemy Enemy { get => enemy; }
    public BattleHurtSource Source { get => source; }
    public int Amount { get => amount; }

    public BattleEntity Target => enemy;

    public void Execute(BattleContext context)
    {
        enemy.ReceiveDamage(amount, source);
    }
}