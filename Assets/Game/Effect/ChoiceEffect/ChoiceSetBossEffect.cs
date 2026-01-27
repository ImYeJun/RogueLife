public class ChoiceSetBossEffect : IChoiceEffect
{
    private EnemyData bossData;

    public ChoiceSetBossEffect(EnemyData bossData)
    {
        if (bossData.Tier != EnemyTier.BOSS) { throw new System.Exception($"The Tire of bossData is Not {EnemyTier.BOSS}."); }
        this.bossData = bossData;
    }

    public void Execute(ChoiceContext context)
    {
        context.ScheduleSystem.SetBoss(bossData);
    }
}