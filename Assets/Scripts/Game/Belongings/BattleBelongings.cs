using UnityEngine;
using ViewEvent.BattleView;

public class BattleBelongings
{
    private BelongingsEntity entity;
    private BelongingsData data;
    private BattleBelongingsBehaviour behaviourInstance;
    private IBattleBelongingsOwner owner;
    private IBattleViewEventPublisher viewEventPublisher;

    public BattleBelongings(BelongingsEntity entity, IBattleBelongingsOwner owner)
    {
        this.entity = entity;
        data = entity.Data;
        behaviourInstance = entity.CloneBattleBehaviour();
        this.owner = owner;
    }

    public Sprite Image => data.Image;
    public string Name => data.BelongingsName;
    public string Description => data.Description;

    public void OnEngageBattle(BattleContext context, IBattleViewEventPublisher viewEventPublisher)
    {
        this.viewEventPublisher = viewEventPublisher;
        behaviourInstance.OnEngageBattle(context, OnExecuted);
    }

    public void OnExecuted()
    {
        viewEventPublisher.Publish(new BelongingsEffectExecuted(this, viewEventPublisher.GetNextSequenceId()));
    }

    public BattleBelongingsBehaviour BehaviourInstance { get => behaviourInstance; }
}