using System.Collections.Generic;
using ViewEvent.BattleView;

public class BattleBelongingsBag : IBattleBelongingsBag, IBattleEventObserveService
{
    private BattleContext context;
    private List<BattleBelongings> belongingsBag = new List<BattleBelongings>();
    private IBattleViewEventPublisher viewEventPublisher;

    public BattleBelongingsBag(IBattleViewEventPublisher viewEventPublisher)
    {
        this.viewEventPublisher = viewEventPublisher;
    }

    public List<BattleBelongings> Belongings => belongingsBag;

    public void SetContext(BattleContext context) 
    { 
        this.context = context; 
    }

    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        eventBus.Subscribe<BattleStartEvent>(Initiate);
    }

    public void Initiate(BattleStartEvent payload)
    {
        this.belongingsBag = payload.BattleBelongings;

        foreach (var belongings in belongingsBag)
        {
            belongings.OnEngageBattle(context);
        }

        viewEventPublisher.Publish(new BelongingsSettled(viewEventPublisher.GetNextSequenceId(), this));
    }
}