public class HurtEnemyBattleAction : IBattleAction
{
    private BattleEnemy enemy;
    private HurtSource source;
    private int amount;

    public HurtEnemyBattleAction(BattleEnemy enemy, HurtSource source, int amount)
    {
        this.enemy = enemy;
        this.source = source;
        this.amount = amount;
    }

    public BattleEnemy Enemy { get => enemy; }
    public HurtSource Source { get => source; }
    public int Amount { get => amount; }

    public void Execute(BattleContext context)
    {
        enemy.ReceiveDamage(amount);
    }
}