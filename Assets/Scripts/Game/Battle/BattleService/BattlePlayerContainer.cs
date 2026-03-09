using ViewEvent.BattleView;

public class BattlePlayerContainer : IBattleEventObserveService, IBattlePlayerContainerContext
{
    private BattlePlayer player;
    private IBattleViewEventPublisher viewEventPublisher;

    public BattlePlayerContainer(IBattleViewEventPublisher viewEventPublisher)
    {
        this.viewEventPublisher = viewEventPublisher;
    }

    public BattlePlayer Player { get => player; }

    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        eventBus.Subscribe<BattleStartEvent>(EnrollPlayer);
    }

    public void EnrollPlayer(BattleStartEvent payload)
    {
        this.player = payload.BattlePlayer;
        this.player.SetViewEventPublisher(viewEventPublisher);
    }
}