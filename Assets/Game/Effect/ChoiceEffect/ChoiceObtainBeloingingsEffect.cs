public class ChoiceObtainBeloingingsEffect : IChoiceEffect
{
    private Belongings obtainingBelongings;

    public ChoiceObtainBeloingingsEffect(Belongings obtainingBelongings)
    {
        this.obtainingBelongings = obtainingBelongings;
    }

    public void Execute(ChoiceContext context)
    {
        context.BelongingsBag.TryObtainBelongings(obtainingBelongings);
    }
}