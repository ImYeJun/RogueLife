public class BattleMaxActionCostModifier
{
    private int delta;
    private BattleScope scope;

    public BattleMaxActionCostModifier(int delta, BattleScope scope)
    {
        this.delta = delta;
        this.scope = scope;
    }

    public int Delta => delta;
    public BattleScope Scope => scope;
}