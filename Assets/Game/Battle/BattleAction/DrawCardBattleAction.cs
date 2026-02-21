using System;

public class  DrawCardBattleAction : IBattleAction
{
    private Guid requestID;
    private Card card;

    public DrawCardBattleAction(Guid requestID, Card card)
    {
        this.requestID = requestID;
        this.card = card;
    }

    public Guid RequestID { get => requestID; }
    public Card Card { get => card; }

    public void Execute(BattleContext context)
    {
        context.ActionScheduler.Enqueue(new MoveCardToDeckBattleAction(card, BattleDeckType.HAND));
    }
}