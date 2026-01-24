public class Belongings
{
    private BelongingsData belongingsData;

    public string Name => belongingsData.BelongingsName;
    public string Description => belongingsData.Description;
    public BelongingsData Data => belongingsData;

    public void Execute(BattleContext context)
    {
        belongingsData.Execute(context);
    }

    public bool Equals(Belongings operand)
    {
        return operand.belongingsData.Equals(belongingsData);
    }
}