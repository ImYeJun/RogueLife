public class TryUseCardBattleAction : IBattleAction
{
    private int cost;
    private Card card;
    private CardTarget cardTarget;

    public TryUseCardBattleAction(int cost, Card card, CardTarget cardTarget)
    {
        this.cost = cost;
        this.card = card;
        this.cardTarget = cardTarget;
    }

    public int Cost { get => cost; }
    public Card Card { get => card; }
    public CardTarget CardTarget { get => cardTarget; }

    public void Execute(BattleContext context)
    {
        if (!card.IsAbleToUse(context, cardTarget)) { return; }
        if (!context.ActionCost.HasEnough(cost)) { return; }

        //* If race condition problem happens, refactor this code to work synchronously.
        context.ActionScheduler.Enqueue(new ConsumeActionCostBattleAction(cost));
        context.ActionScheduler.Enqueue(new UseCardBattleAction(card, cardTarget));
    }
}