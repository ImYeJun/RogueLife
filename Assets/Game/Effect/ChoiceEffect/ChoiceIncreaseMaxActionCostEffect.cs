public class ChoiceIncreaseMaxActionCostEffect : IChoiceEffect
{
    private int amount;

    public ChoiceIncreaseMaxActionCostEffect(int amount)
    {
        this.amount = amount;
    }

    public void Execute(ChoiceContext context)
    {
        context.ActionCost.IncreaseMaxActionCost(amount);
    }
}