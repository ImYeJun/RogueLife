public class RequestTryTriggerCardBattleAction : IBattleAction
{
    private Card card;

    public RequestTryTriggerCardBattleAction(Card card)
    {
        this.card = card;
    }

    public void Execute(BattleContext context)
    {
        UnityEngine.Debug.LogError("[RequestTryTriggerCardBattleAction] RequestTryTriggerCardBattleAction is not implemented!");
        // context.ActionScheduler.Pause();
        // context.UIManager.TriggerCard(card);
    }
}