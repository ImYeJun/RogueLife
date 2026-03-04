using System;

public class Belongings
{
    private BelongingsEntity entity;
    private BelongingsData data;
    private FieldBelongingsBehaviour behaviourInstance;

    public Belongings(BelongingsEntity entity)
    {
        this.entity = entity;
        data = entity.Data;
        behaviourInstance = entity.CloneFieldBehaviour();
    }

    public Belongings(Belongings belongings)
    {
        entity = belongings.entity;
        data = entity.Data;
        behaviourInstance = entity.CloneFieldBehaviour();
    }

    public string Name => data.BelongingsName;
    public string Description => data.Description;
    public BelongingsData Data => data;
    public BelongingsEntity Entity { get => entity; }

    public void OnEquipped(FieldContext context) { behaviourInstance.OnEquipped(context); }
    public void OnUnequipped(FieldContext context) { behaviourInstance.OnUnqeuipped(context); }

    public BattleBelongings GenerateBattleBelongings(IBattleBelongingsOwner owner) { return new BattleBelongings(entity, owner); }

    public bool Equals(Belongings operand)
    {
        return operand.data.Equals(data);
    }
}