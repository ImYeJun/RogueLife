public class ChoiceDecreaseMaxActionCostEffect : IChoiceEffect
{
    private int amount;

    public ChoiceDecreaseMaxActionCostEffect(int amount)
    {
        this.amount = amount;
    }

    public void Execute(ChoiceContext context)
    {
        context.ActionCost.DecreaseMaxActionCost(amount);
    }
}