using UnityEngine;

public class TryUseCardBattleAction : IBattleAction
{
    private int cost;
    private Card card;
    private CardTarget cardTarget;
    private bool hasNullified;
    private bool isSuccess;

    public TryUseCardBattleAction(int cost, Card card, CardTarget cardTarget)
    {
        this.cost = cost;
        this.card = card;
        this.cardTarget = cardTarget;
        hasNullified = false;
        isSuccess = false;
    }

    public int Cost { get => cost; }
    public Card Card { get => card; }
    public CardTarget CardTarget { get => cardTarget; }
    public bool IsSuccess { get => isSuccess; }

    public void Execute(BattleContext context)
    {
        if (hasNullified)
        {
            //TODO This code is the same with UseCardEffectBattleAction code. BULLSHIT REFACTOR IT. 
            OnNullified(context);
            return;
        }
        if (!card.IsAbleToUse(context, cardTarget)) { return; }
        if (!context.ActionCost.HasEnough(cost)) { return; }

        context.ActionScheduler.EnqueueFront(new UseCardBattleAction(card, cardTarget));
        context.ActionScheduler.EnqueueFront(new ConsumeActionCostBattleAction(cost));

        isSuccess = true;
    }

    private void OnNullified(BattleContext context)
    {
        var destination = card.IsReflectionApplied ? BattleDeckType.DRAW : BattleDeckType.GRAVE;
        context.ActionScheduler.EnqueueFront(new NotifyCardExecutionCompletedBattleAction(card));
        context.ActionScheduler.EnqueueFront(new MoveCardToDeckBattleAction(card, destination));
    }

    public void Nullify()
    {
        hasNullified = true;
    }

    public void ReduceCost(int amount)
    {
        cost = Mathf.Max(cost - amount, 0);
    }

    public void IncreaseCost(int amount)
    {
        cost += amount;
    }
}