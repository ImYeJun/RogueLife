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
        UnityEngine.Debug.LogError("[RequestTryTriggerCardBattleAction] RequestTryTriggerCardBattleAction is not implemented!");
        // context.ActionScheduler.Pause();
        // context.UIManager.TriggerCard(card);
    }
}