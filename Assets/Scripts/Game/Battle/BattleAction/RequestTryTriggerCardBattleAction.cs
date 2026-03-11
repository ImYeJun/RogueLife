public class RequestTryTriggerCardBattleAction : IBattleAction
{
    private Card card;
    private bool isReflection;

    public RequestTryTriggerCardBattleAction(Card card, bool isReflection)
    {
        this.card = card;
        this.isReflection = isReflection;
    }

    public void Execute(BattleContext context)
    {
        context.ActionScheduler.Pause();
        context.DeckSystem.RequestTriggerCard(card, isReflection);
    }
}