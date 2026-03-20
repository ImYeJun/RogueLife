using System;

public class RemoveCardCostModifierBattleAction : IBattleAction
{
    private Card targetCard;
    private CardCostModifier modifier;

    public RemoveCardCostModifierBattleAction(Card targetCard, CardCostModifier modifier)
    {
        this.targetCard = targetCard;
        this.modifier = modifier;
    }

    public void Execute(BattleContext context)
    {
        if (targetCard != null && modifier != null)
        {
            targetCard.RemoveCostModifier(modifier);
            context.EventBus.Publish(new CardCostChangedBattleEvent(targetCard, targetCard.CurrentActionCost));
        }
    }
}