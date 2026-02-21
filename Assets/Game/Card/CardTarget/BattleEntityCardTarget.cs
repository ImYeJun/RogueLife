public class BattleEntityCardTarget : CardTarget
{
    private BattleEntity entity;

    public BattleEntityCardTarget(BattleEntity entity)
    {
        this.entity = entity;
    }

    public BattleEntity Entity { get => entity; }
}