public class RequestHurtEntityBattleAction : IBattleAction
{
    private HurtSource source;
    private int amount;
    private BattleEntity target;

    public RequestHurtEntityBattleAction(HurtSource source, int amount, BattleEntity target)
    {
        this.source = source;
        this.amount = amount;
        this.target = target;
    }

    public HurtSource Source { get => source; }
    public int Amount { get => amount; }
    public BattleEntity Target { get => target; }

    public void Execute(BattleContext context)
    {
        target.RequestHurt(amount, source);
    }
}