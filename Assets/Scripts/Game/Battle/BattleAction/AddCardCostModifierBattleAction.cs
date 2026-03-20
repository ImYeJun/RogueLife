using System;

public class AddCardCostModifierBattleAction : IBattleAction
{
    private Card targetCard;
    private CardCostModifier modifier;

    public AddCardCostModifierBattleAction(Card targetCard, CardCostModifier modifier)
    {
        this.targetCard = targetCard;
        this.modifier = modifier;
    }

    public void Execute(BattleContext context) 
    {
        if (targetCard != null && modifier != null)
        {
            targetCard.AddCostModifier(modifier);
            context.EventBus.Publish(new CardCostChangedBattleEvent(targetCard, targetCard.CurrentActionCost));
        }
    }
}