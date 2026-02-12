public class FulfillActionCostBattleAction : IBattleAction
{
    public void Execute(BattleContext context)
    {
        context.ActionCost.Fullfill();
    }
}