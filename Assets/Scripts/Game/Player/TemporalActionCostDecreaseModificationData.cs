public class TemporalActionCostDecreaseModifier
{
    private int remainBattleCount;
    private int modificatedAmount;

    public TemporalActionCostDecreaseModifier(int remainBattleCount, int modificatedAmount)
    {
        this.remainBattleCount = remainBattleCount;
        this.modificatedAmount = modificatedAmount;
    }

    public int RemainBattleCount { get => remainBattleCount; set => remainBattleCount = value; }
    public int ModificatedAmount { get => modificatedAmount; }
}