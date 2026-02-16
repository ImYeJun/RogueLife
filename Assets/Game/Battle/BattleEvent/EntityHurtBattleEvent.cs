public class EntityHurtBattleEvent : BattleEvent
{
    private int amount;
    private BattleEntity victim;
    private BattleHurtSource source;

    public EntityHurtBattleEvent(int amount, BattleEntity victim, BattleHurtSource source)
    {
        this.amount = amount;
        this.victim = victim;
        this.source = source;
    }

    public int Amount { get => amount; }
    public BattleEntity Victim { get => victim; }
    public BattleHurtSource Source { get => source; }
}