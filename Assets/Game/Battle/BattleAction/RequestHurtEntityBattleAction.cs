public class RequestHurtEntityBattleAction : IBattleAction, IEntityTargetedBattleAction
{
    private BattleHurtSource source;
    private int amount;
    private BattleEntity target;

    public RequestHurtEntityBattleAction(BattleHurtSource source, int amount, BattleEntity target)
    {
        this.source = source;
        this.amount = amount;
        this.target = target;
    }

    public BattleHurtSource Source { get => source; }
    public int Amount { get => amount; }
    public BattleEntity Target { get => target; }

    public void Execute(BattleContext context)
    {
        target.RequestHurt(amount, source);
    }
}