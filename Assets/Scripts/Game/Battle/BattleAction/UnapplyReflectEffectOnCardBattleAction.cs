using System;

public class UnapplyReflectEffectOnCardBattleAction : IBattleAction
{
    private Card card;
    public UnapplyReflectEffectOnCardBattleAction(Card card)
    {
        this.card = card;
    }
    public void Execute(BattleContext context)
    {
        card.UnapplyReflection();
        context.EventBus.Publish(new CardReflectionChangedBattleEvent(card, card.IsReflectionApplied));
    }
}