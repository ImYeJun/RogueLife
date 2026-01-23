public class Belongings
{
    private BelongingsData belongingsData;

    public string Name => belongingsData.BelongingsName;

    public string GetDescription()
    {
        return "munegaooki";
    }

    public void Execute(BattleContext context)
    {
        
    }

    public bool Equals(Belongings operand)
    {
        return operand.belongingsData.Equals(belongingsData);
    }
}