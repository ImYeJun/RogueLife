public class BattlePlayerContainer : IBattleEventObserveService, IBattlePlayerContainerContext
{
    private BattlePlayer player;

    public BattlePlayer Player { get => player; }

    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        eventBus.Subscribe<BattleStartEvent>(EnrollPlayer);
    }
    public void EnrollPlayer(BattleStartEvent payload)
    {
        player = payload.BattlePlayer;
    }
}