public class EnemyDataSlot
{
    private EnemyEntity entity;

    public EnemyDataSlot(EnemyEntity entity)
    {
        this.entity = entity;
    }

    public EnemyEntity Entity { get => entity; set => entity = value; }
}