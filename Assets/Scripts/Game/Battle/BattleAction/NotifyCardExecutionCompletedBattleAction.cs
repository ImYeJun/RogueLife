public class NotifyCardExecutionCompletedBattleAction : IBattleAction
{
    private Card card;
    public NotifyCardExecutionCompletedBattleAction(Card card) { this.card = card; }
    public void Execute(BattleContext context)
    {
        context.EventBus.Publish(new CardExecutionCompletedBattleEvent(card));
    }
}