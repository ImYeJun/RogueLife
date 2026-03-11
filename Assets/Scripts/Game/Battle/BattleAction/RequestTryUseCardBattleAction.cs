public class RequestTryUseCardBattleAction : IBattleAction
{
    private Card card;
    private bool isFreeUse;

    public RequestTryUseCardBattleAction(Card card, bool isFreeUse = false)
    {
        this.card = card;
        this.isFreeUse = isFreeUse;
    }

    public void Execute(BattleContext context)
    {
        context.ActionScheduler.Pause();
        context.DeckSystem.RequestUseCard(card, isFreeUse);
    }
}