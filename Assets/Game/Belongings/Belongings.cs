using System;

[Serializable]
public class Belongings
{
    private BelongingsData data;
    private FieldBelongingsBehaviour behaviourInstance;

    public Belongings(BelongingsData data)
    {
        this.data = data;
        behaviourInstance = this.data.CloneFieldBehaviour();
    }

    public Belongings(Belongings belongings)
    {
        data = belongings.data;
        behaviourInstance = data.CloneFieldBehaviour();
    }

    public string Name => data.BelongingsName;
    public string Description => data.Description;
    public BelongingsData Data => data;

    public void OnEquipped(FieldContext context) { behaviourInstance.OnEquipped(context); }
    public void OnUnequipped(FieldContext context) { behaviourInstance.OnUnqeuipped(context); }

    public BattleBelongings GenerateBattleBelongings(IBattleBelongingsOwner owner) { return new BattleBelongings(data, owner); }

    public bool Equals(Belongings operand)
    {
        return operand.data.Equals(data);
    }
}