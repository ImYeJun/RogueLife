using System;

public class RequestDrawCardBattleAction : IBattleAction
{
    private CardRarity rarity;
    private CardAttribute attribute;
    private CardType type;
    private Guid requestId;

    public RequestDrawCardBattleAction(CardRarity rarity, CardAttribute attribute, CardType type, Guid requestId)
    {
        this.rarity = rarity;
        this.attribute = attribute;
        this.type = type;
        this.requestId = requestId;
    }

    public CardRarity Rarity { get => rarity; }
    public CardAttribute Attribute { get => attribute; }
    public CardType Type { get => type; }
    public Guid RequestId { get => requestId; }

    public void Execute(BattleContext context)
    {
        var drawingCard = context.DeckSystem.RequestDrawingCard(context.Random, rarity, attribute, type);

        if (drawingCard == null) { return; }
        context.ActionScheduler.Enqueue(new DrawCardBattleAction(requestId, drawingCard));
    }
}