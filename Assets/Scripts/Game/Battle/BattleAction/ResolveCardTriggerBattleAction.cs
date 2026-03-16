using System;

public class ResolveCardTriggerBattleAction : IBattleAction
{
    private Card card;

    public ResolveCardTriggerBattleAction(Card card)
    {
        this.card = card;
    }

    public void Execute(BattleContext context)
    {
        context.DeckSystem.RemoveActiveTriggerCard(card);
    }
}