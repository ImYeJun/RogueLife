using System;

public class ApplyReflectEffectOnCard : IBattleAction
{
    private Card card;
    public ApplyReflectEffectOnCard(Card card)
    {
        this.card = card;
    }
    public void Execute(BattleContext context)
    {
        card.ApplyReflection();
    }
}