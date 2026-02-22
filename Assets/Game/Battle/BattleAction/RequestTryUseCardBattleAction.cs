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
        UnityEngine.Debug.LogError("[RequestTryUseCardBattleAction] RequestTryUseCardBattleAction is not implemented!");
        // context.ActionScheduler.Pause();
        // context.UIManamger.UseCard(card);
    }
}