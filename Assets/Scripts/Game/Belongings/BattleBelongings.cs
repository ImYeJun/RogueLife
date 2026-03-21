using UnityEngine;

public class BattleBelongings
{
    private BelongingsEntity entity;
    private BelongingsData data;
    private BattleBelongingsBehaviour behaviourInstance;
    private IBattleBelongingsOwner owner;

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

    public void OnEngageBattle(BattleContext context)
    {
        behaviourInstance.OnEngageBattle(context);
    }

    public BattleBelongingsBehaviour BehaviourInstance { get => behaviourInstance; }
}