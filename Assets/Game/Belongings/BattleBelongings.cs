public class BattleBelongings
{
    private BelongingsData data;
    private BattleBelongingsBehaviour behaviourInstance;
    private IBattleBelongingsOwner owner;

    public BattleBelongings(BelongingsData data, IBattleBelongingsOwner owner)
    {
        this.data = data;
        behaviourInstance = this.data.CloneBattleBehaviour();
        this.owner = owner;
    }

    public BattleBelongingsBehaviour BehaviourInstance { get => behaviourInstance; }
}