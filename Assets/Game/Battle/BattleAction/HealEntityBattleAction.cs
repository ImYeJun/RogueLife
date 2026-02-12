public class HealEntityBattleAction : IBattleAction
{
    private BattleEntity target;
    private int amount;

    public HealEntityBattleAction(BattleEntity target, int amount)
    {
        this.target = target;
        this.amount = amount;
    }

    public BattleEntity Target { get => target; }
    public int Amount { get => amount; }

    public void Execute(BattleContext context)
    {
        target.Heal(amount);
    }
}