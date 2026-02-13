public class EnemyDataSlot
{
    private EnemyData data;

    public EnemyDataSlot(EnemyData data)
    {
        this.data = data;
    }

    public EnemyData Data { get => data; set => data = value; }
}