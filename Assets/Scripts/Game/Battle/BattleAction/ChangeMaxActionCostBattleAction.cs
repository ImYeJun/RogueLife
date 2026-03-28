public class ChangeMaxActionCostBattleAction : IBattleAction
{
    private BattleMaxActionCostModifier modifier;

    public ChangeMaxActionCostBattleAction(BattleMaxActionCostModifier modifier)
    {
        this.modifier = modifier;
    }

    public void Execute(BattleContext context)
    {
        context.ActionCost.AddModifier(modifier);
    }
}