public class SchedulePathCountRule
{
    private int minCompeletePath;
    private int maxCompletePath;

    public SchedulePathCountRule(int minCompeletePath, int maxCompletePath)
    {
        this.minCompeletePath = minCompeletePath;
        this.maxCompletePath = maxCompletePath;
    }

    public int MinCompeletePath { get => minCompeletePath; }
    public int MaxCompletePath { get => maxCompletePath; }
}