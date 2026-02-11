public class EntityHurtBattleEvent : BattleEvent
{
    private int amount;
    private BattleEntity victim;
    private HurtSource source;

    public EntityHurtBattleEvent(int amount, BattleEntity victim, HurtSource source)
    {
        this.amount = amount;
        this.victim = victim;
        this.source = source;
    }

    public int Amount { get => amount; }
    public BattleEntity Victim { get => victim; }
    public HurtSource Source { get => source; }
}