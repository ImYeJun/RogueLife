public class RequestTryUseCardBattleAction : IBattleAction
{
    private Card card;

    public RequestTryUseCardBattleAction(Card card)
    {
        this.card = card;
    }

    public void Execute(BattleContext context)
    {
        UnityEngine.Debug.LogError("[RequestTryUseCardBattleAction] RequestTryUseCardBattleAction is not implemented!");
        // context.ActionScheduler.Pause();
        // context.UIManamger.UseCard(card);
    }
}