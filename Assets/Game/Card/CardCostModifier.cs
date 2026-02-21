public class CardCostModifier
{
    private int delta;

    public CardCostModifier(int delta)
    {
        this.delta = delta;
    }

    public int Delta { get => delta; }
}