public class UseCardEffectBattleAction : IBattleAction
{
    private Card card;
    private CardTarget cardTarget;

    public UseCardEffectBattleAction(Card card, CardTarget cardTarget)
    {
        this.card = card;
        this.cardTarget = cardTarget;
    }

    public Card Card { get => card; }
    public CardTarget CardTarget { get => cardTarget; }

    public void Execute(BattleContext context)
    {
        card.Execute(context, cardTarget);
    }
}