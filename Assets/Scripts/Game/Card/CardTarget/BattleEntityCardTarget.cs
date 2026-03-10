public class BattleEntityCardTarget : CardTarget
{
    private BattleEntity entity;

    public BattleEntityCardTarget(IReadOnlyBattleEntity entity)
    {
        this.entity = (BattleEntity)entity;
        //TODO Hard Refactor is needed to remove the hard Casting
    }

    public BattleEntity Entity { get => entity; }
}