using System;

public class RequestDrawCardBattleAction : IBattleAction
{
    private CardAttribute attribute;
    private CardType type;
    private Guid requestId;

    public RequestDrawCardBattleAction(CardAttribute attribute, CardType type, Guid requestId)
    {
        this.attribute = attribute;
        this.type = type;
        this.requestId = requestId;
    }

    public CardAttribute Attribute { get => attribute; }
    public CardType Type { get => type; }
    public Guid RequestId { get => requestId; }

    public void Execute(BattleContext context)
    {
        var drawingCard = context.DeckSystem.RequestDrawingCard(context.Random, attribute, type);

        if (drawingCard == null) { return; }
        context.ActionScheduler.Enqueue(new DrawCardBattleAction(requestId, drawingCard));
    }
}